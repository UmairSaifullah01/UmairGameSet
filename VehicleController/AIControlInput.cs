using PathCreation;
using UnityEngine;


namespace UMGS.Vehicle
{


	public class AIControlInput : ControlInput
	{

		public                                   Transform Target;
		[SerializeField] [Range(0, 1)]   private float     m_CautiousSpeedFactor           = 0.05f; // percentage of max speed to use when being maximally cautious
		[SerializeField] [Range(0, 180)] private float     m_CautiousMaxAngle              = 50f;
		[SerializeField]                 private float     m_SteerSensitivity              = 0.05f;
		[SerializeField]                 private float     m_CautiousAngularVelocityFactor = 30f;
		[SerializeField]                         Rigidbody _rigidbody;

		public override void DoUpdate(float speed)
		{
			Vector3 offsetTargetPos        = Target.position;
			Vector3 fwd                    = transform.forward;
			float   desiredSpeed           = 80f;
			float   approachingCornerAngle = Vector3.Angle(Target.forward, fwd);
			float   spinningAngle          = _rigidbody.angularVelocity.magnitude * m_CautiousAngularVelocityFactor;
			float   cautiousnessRequired   = Mathf.InverseLerp(0, m_CautiousMaxAngle, Mathf.Max(spinningAngle, approachingCornerAngle));
			desiredSpeed = Mathf.Lerp(80, 80 * m_CautiousSpeedFactor, cautiousnessRequired);
			float   accel       = Mathf.Clamp((desiredSpeed - speed) * 0.04f, -1, 1);
			Vector3 localTarget = transform.InverseTransformPoint(offsetTargetPos);
			float   targetAngle = Mathf.Atan2(localTarget.x, localTarget.z)            * Mathf.Rad2Deg;
			float   steer       = Mathf.Clamp(targetAngle * m_SteerSensitivity, -1, 1) * Mathf.Sign(speed);
			throttle = accel;
			turn     = steer;
		}

	}


}