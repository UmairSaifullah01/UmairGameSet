using System;
using EVP;
using UnityEngine;
using Object = UnityEngine.Object;


namespace UMGS.Vehicle
{


	[UClassHeader("Vehicle Engine")]
	public class VehicleEngine : UBehaviour
	{

		// SerializeFields
		[UToolbar("Engine")] [SerializeField]                       AnimationCurve motorTorque    = new AnimationCurve(new Keyframe(0, 200), new Keyframe(50, 300), new Keyframe(200, 0));
		[SerializeField]                                            AnimationCurve turnInputCurve = AnimationCurve.Linear(-1.0f, -1.0f, 1.0f, 1.0f);
		[Range(2,    16)] [SerializeField]                          float          diffGearing    = 4.0f;
		[Range(0.5f, 15f)] [SerializeField]                         float          downforce      = 1.0f;
		[SerializeField]                                            float          antiRoll;
		[SerializeField]                                            float          maxSpeed   = 80.0f;
		[SerializeField]                                            float          brakeForce = 1500.0f;
		[Range(0f,     50.0f)] [SerializeField]                     float          steerAngle = 30.0f;
		[Range(0.001f, 1.0f)] [SerializeField]                      float          steerSpeed = 0.2f;
		[SerializeField]                                            Transform      centerOfMass;
		[UToolbar("Wheels")] [SerializeField]                       Wheel[]        wheels;
		[Header("Easy Suspension")] [Range(0, 20)] [SerializeField] float          naturalFrequency = 10;

		[Range(0, 3)] [SerializeField] float dampingRatio = 0.8f;

		[Range(-1, 1)] [SerializeField] float forceShift = 0.03f;

		public                                  bool             setSuspensionDistance = true;
		[UToolbar("Audio")] [SerializeField]    VehicleAudio     sounds;
		[UToolbar("Lights")] [SerializeField]   VehicleLights    Lights;
		[UToolbar("Effects")] [SerializeField]  ParticleSystem[] gasParticles;
		[UToolbar("Controls")] [SerializeField] ControlInput     ControlInput;
		// Public Area 
		public float speed          => transform.InverseTransformDirection(attachedRigidbody.velocity).z * 3.6f;
		public float directionSpeed => Vector3.Dot(attachedRigidbody.velocity, transform.forward);
		public bool IsGrounded
		{
			get
			{
				isGrounded = true;
				foreach (Wheel wheel in wheels)
				{
					if (Physics.Raycast(wheel.WheelCollider.transform.position, -wheel.WheelCollider.transform.up, 2))
					{
						isGrounded = true;
						return isGrounded;
					}
				}

				isGrounded = false;
				return isGrounded;
			}
		}
		[HideInInspector] public Rigidbody attachedRigidbody;
		//public bool IsGrounded
		//{
		//	get
		//	{
		//			if (lastGroundCheck == Time.frameCount)
		//				return isGrounded;
		//			lastGroundCheck = Time.frameCount;
		//			isGrounded      = true;
		//			foreach (Wheel wheel in wheels)
		//			{
		//				if (!wheel.WheelCollider.gameObject.activeSelf || !wheel.WheelCollider.isGrounded)
		//					isGrounded = false;
		//			}
//
		//			return isGrounded;
		//		} 
		public float lastCollisionTime { get; private set; }

		//}
		//private 

		bool isGrounded      = false;
		int  lastGroundCheck = 0;
		bool isColliding     = false;

		void Start()
		{
			attachedRigidbody            = GetComponent<Rigidbody>();
			attachedRigidbody.useGravity = false;
			if (attachedRigidbody != null && centerOfMass != null)
			{
				attachedRigidbody.centerOfMass = centerOfMass.localPosition;
			}

			sounds.Initialize(ControlInput);
		}

		void Update()
		{
			foreach (ParticleSystem gasParticle in gasParticles)
			{
				gasParticle.Play();
				ParticleSystem.EmissionModule em = gasParticle.emission;
				em.rateOverTime = ControlInput.handbrake > 0.5f ? 0 : Mathf.Lerp(em.rateOverTime.constant, Mathf.Clamp(150.0f * ControlInput.throttle, 30.0f, 100.0f), 0.1f);
			}

			if (isColliding) lastCollisionTime += Time.deltaTime;
			sounds.DoUpdate(speed);
		}

		void FixedUpdate()
		{
			ControlInput.DoUpdate(directionSpeed);
			Lights.DoUpdate(ControlInput.throttle, ControlInput.brake, ControlInput.handbrake > 0.5f ? 1 : 0, ControlInput.turn);
			float steerValue = ComputeSteerAngle();
			foreach (Wheel wheel in wheels)
			{
				if (wheel.Steer) wheel.WheelCollider.steerAngle = steerValue;
				if (ControlInput.handbrake > 0.5f)
				{
					// Don't zero out this value or the wheel completely lock up
					wheel.WheelCollider.motorTorque = 0.0001f;
					wheel.WheelCollider.brakeTorque = brakeForce;
				}
				else if (wheel.Drive && ControlInput.brake < 0.5f)
				{
					wheel.WheelCollider.motorTorque = ControlInput.throttle * motorTorque.Evaluate(speed) * diffGearing / 4; //Remember to count drive wheels
				}

				wheel.WheelCollider.brakeTorque = brakeForce * ControlInput.brake;
				VisualWheelsUpdate(wheel);
				//Suspension Calculations 
				EasySuspension(wheel.WheelCollider);
			}

			//max Speed Counter
			attachedRigidbody.velocity = attachedRigidbody.velocity.normalized * Mathf.Clamp(attachedRigidbody.velocity.magnitude, 0, maxSpeed * (1 - ControlInput.brake));
			ApplyAntiRoll();
			//Grounded Checking....
			if (!IsGrounded)
			{
				attachedRigidbody.AddForce(Vector3.Cross(UMTools.SetVector3Axis(attachedRigidbody.velocity, 0, Axis.y), Vector3.up) * (-ControlInput.turn * 500));
				//Simple Gravity
				attachedRigidbody.AddForce(Vector3.down      * (9.8f * attachedRigidbody.mass));
				transform.Rotate(0, 0, Time.deltaTime * 0.1f * -ControlInput.turn);
				attachedRigidbody.angularVelocity = Vector3.zero;
			}
			else
			{
				//Downforce
				attachedRigidbody.AddForce(-transform.up * (speed * downforce * 10));
				//Simple Gravity
				if (speed < 0.2f) attachedRigidbody.AddForce(Vector3.down * (9.8f * attachedRigidbody.mass));
				else //Stick Gravity
					attachedRigidbody.AddForce(-transform.up * (9.8f * attachedRigidbody.mass));
			}
		}

		float[] travelValues = new float[4];

		void ApplyAntiRoll()
		{
			WheelHit hit;
			for (int i = 0; i < wheels.Length; i++)
			{
				isGrounded      = wheels[i].WheelCollider.GetGroundHit(out hit);
				travelValues[i] = isGrounded ? (-wheels[i].WheelCollider.transform.InverseTransformPoint(hit.point).y - wheels[i].WheelCollider.radius) / wheels[i].WheelCollider.suspensionDistance : 1;
			}

			float antiRollForceFrontHorizontal = (travelValues[0] - travelValues[1]) * antiRoll;
			if (wheels[0].WheelCollider.isGrounded)
				attachedRigidbody.AddForceAtPosition(wheels[0].WheelCollider.transform.up * -antiRollForceFrontHorizontal, wheels[0].WheelCollider.transform.position);
			if (wheels[1].WheelCollider.isGrounded) attachedRigidbody.AddForceAtPosition(wheels[1].WheelCollider.transform.up * -antiRollForceFrontHorizontal, wheels[1].WheelCollider.transform.position);
			antiRollForceFrontHorizontal = (travelValues[2] - travelValues[3]) * antiRoll;
			if (wheels[2].WheelCollider.isGrounded) attachedRigidbody.AddForceAtPosition(wheels[2].WheelCollider.transform.up * -antiRollForceFrontHorizontal, wheels[2].WheelCollider.transform.position);
			if (wheels[3].WheelCollider.isGrounded) attachedRigidbody.AddForceAtPosition(wheels[3].WheelCollider.transform.up * -antiRollForceFrontHorizontal, wheels[3].WheelCollider.transform.position);
			float antiRollForceFrontVertical = (travelValues[0] - travelValues[2]) * antiRoll;
			if (wheels[0].WheelCollider.isGrounded) attachedRigidbody.AddForceAtPosition(wheels[0].WheelCollider.transform.up * -antiRollForceFrontVertical, wheels[0].WheelCollider.transform.position);
			if (wheels[2].WheelCollider.isGrounded) attachedRigidbody.AddForceAtPosition(wheels[2].WheelCollider.transform.up * -antiRollForceFrontVertical, wheels[2].WheelCollider.transform.position);
			antiRollForceFrontVertical = (travelValues[1] - travelValues[3]) * antiRoll;
			if (wheels[1].WheelCollider.isGrounded) attachedRigidbody.AddForceAtPosition(wheels[1].WheelCollider.transform.up * -antiRollForceFrontVertical, wheels[1].WheelCollider.transform.position);
			if (wheels[3].WheelCollider.isGrounded) attachedRigidbody.AddForceAtPosition(wheels[3].WheelCollider.transform.up * -antiRollForceFrontVertical, wheels[3].WheelCollider.transform.position);
		}

		void VisualWheelsUpdate(Wheel wheel)
		{
			wheel.WheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
			wheel.VisualWheel.rotation      =  rot;
			wheel.VisualWheel.localRotation *= Quaternion.Euler(wheel.LocalRotOffset);
			wheel.VisualWheel.position      =  pos;
		}

		void EasySuspension(WheelCollider wc)
		{
			JointSpring spring = wc.suspensionSpring;
			spring.spring       = Mathf.Pow(Mathf.Sqrt(wc.sprungMass) * naturalFrequency, 2);
			spring.damper       = 2 * dampingRatio * Mathf.Sqrt(spring.spring * wc.sprungMass);
			wc.suspensionSpring = spring;
			Vector3 wheelRelativeBody = transform.InverseTransformPoint(wc.transform.position);
			float   distance          = attachedRigidbody.centerOfMass.y - wheelRelativeBody.y + wc.radius;
			wc.forceAppPointDistance = distance - forceShift;

			// the following line makes sure the spring force at maximum droop is exactly zero
			if (spring.targetPosition > 0 && setSuspensionDistance)
				wc.suspensionDistance = wc.sprungMass * Physics.gravity.magnitude / (spring.targetPosition * spring.spring);
		}

		float ComputeSteerAngle()
		{
			float inputSteerAngle = steerAngle * turnInputCurve.Evaluate(ControlInput.turn);
			float speedFactor     = Mathf.Lerp(0.1f, 1.0f, 1 - (attachedRigidbody.velocity.magnitude / maxSpeed));
			inputSteerAngle *= speedFactor;
			float assistedSteerAngle = steerAngle * 0.6f * (speedFactor / 2) * Mathf.InverseLerp(1.0f, 3.0f, Mathf.Abs(steerSpeed));
			return Mathf.Clamp(Mathf.Lerp(0, inputSteerAngle + assistedSteerAngle, Time.deltaTime * 100 * steerSpeed), -steerAngle, +steerAngle);
		}

		public void OnSkid(Vector3 skidPoint, float intensity)
		{
			sounds.SkidAudio(skidPoint, intensity);
		}

		void OnCollisionEnter(Collision other)
		{
			isColliding       = true;
			lastCollisionTime = 0;
			sounds.ImpactAudio(other.contacts[0].point);
		}

		void OnCollisionExit(Collision other)
		{
			isColliding = false;
		}

	}

	[System.Serializable]
	public struct Wheel
	{

		public WheelCollider WheelCollider;
		public Transform     VisualWheel;
		public Vector3       LocalRotOffset;
		public bool          Drive, Steer;

	}

	[System.Serializable]
	public class VehicleAudio
	{

		[Header("Pitch Parameter")] public float       flatoutSpeed = 20.0f;
		[Range(0.0f, 3.0f)]         public float       minPitch     = 0.7f;
		[Range(0.0f, 0.1f)]         public float       pitchSpeed   = 0.05f;
		[Header("Clips")]           public AudioClip   rolling;
		public                             AudioClip   starting, impact, skid;
		[Header("Clips")] public           AudioSource engineSource;
		public                             AudioSource othersSource;
		ControlInput                                   _input;
		bool                                           startUp = false;

		public void Initialize(ControlInput input)
		{
			_input = input;
		}

		public void DoUpdate(float speed)
		{
			if (_input.run && !startUp)
			{
				engineSource.clip = starting;
				engineSource.Play();
				engineSource.pitch = 1;
				startUp            = true;
			}

			if (startUp && !engineSource.isPlaying)
			{
				engineSource.clip = rolling;
				engineSource.loop = true;
				engineSource.Play();
			}

			if (engineSource.clip == rolling)
			{
				engineSource.pitch = Mathf.Clamp(Mathf.Lerp(engineSource.pitch, minPitch + Mathf.Abs(speed) / flatoutSpeed, pitchSpeed), minPitch, 2);
			}
		}

		public void ImpactAudio(Vector3 atPoint)
		{
			PlayOneTime(impact, atPoint, 0.5f, 1);
		}

		public void SkidAudio(Vector3 atPoint, float volume)
		{
			PlayOneTime(skid, atPoint, volume * 0.5f, 1);
		}

		void PlayOneTime(AudioClip clip, Vector3 position, float volume, float pitch)
		{
			if (pitch < 0.01f || volume < 0.01f) return;
			AudioSource source = Object.Instantiate(othersSource, position, Quaternion.identity, othersSource.transform.parent);
			if (source.isActiveAndEnabled)
			{
				source.clip         = clip;
				source.volume       = volume;
				source.pitch        = pitch;
				source.dopplerLevel = 0.0f; // Doppler causes artifacts as for positioning the audio source
				source.Play();
			}

			Object.Destroy(source.gameObject, clip.length / pitch);
		}

	}


}