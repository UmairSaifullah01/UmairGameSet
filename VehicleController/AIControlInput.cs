using System;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;


namespace UMGS.Vehicle
{


	[RequireComponent(typeof(VehicleEngine))]
	public class AIControlInput : ControlInput
	{

		[SerializeField]                         Transform path;
		[SerializeField] [Range(0, 1)]   private float     m_CautiousSpeedFactor           = 0.05f; // percentage of max speed to use when being maximally cautious
		[SerializeField] [Range(0, 180)] private float     m_CautiousMaxAngle              = 50f;
		[SerializeField]                 private float     m_SteerSensitivity              = 0.05f;
		[SerializeField]                 private float     m_AccelSensitivity              = 0.04f;
		[SerializeField]                 private float     m_CautiousAngularVelocityFactor = 30f;

		[SerializeField] float MaxSpeed, OffsetX;

		int               currentNode;
		List<Transform>   nodes;
		VehicleEngine     _engine;
		Rigidbody         _rigidbody => _engine.attachedRigidbody;
		private Transform Target;

		void Start()
		{
			_engine = GetComponent<VehicleEngine>();
			Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
			nodes = new List<Transform>();
			for (int i = 0; i < pathTransforms.Length; i++)
			{
				if (pathTransforms[i] != path.transform)
				{
					nodes.Add(pathTransforms[i]);
				}
			}

			_rigidbody.OnTriggerEnter((col) =>
			{
				if (col.CompareTag("Respawn"))
				{
					ResetPosition();
				}
			});
		}

		public override void DoUpdate(float speed)
		{
			if (currentNode == -1)
			{
				Stop();
				return;
			}

			CheckWaypointDistance();
			Vector3 offsetTargetPos        = new Vector3(Target.position.x + OffsetX, Target.position.y, Target.position.z);
			Vector3 fwd                    = transform.forward;
			//Accelaration Calculations
			float   desiredSpeed           = MaxSpeed;
			float   approachingCornerAngle = Vector3.Angle(Target.forward, fwd);
			float   spinningAngle          = _rigidbody.angularVelocity.magnitude * m_CautiousAngularVelocityFactor;
			float   cautiousnessRequired   = Mathf.InverseLerp(0, m_CautiousMaxAngle, Mathf.Max(spinningAngle, approachingCornerAngle));
			desiredSpeed = Mathf.Lerp(MaxSpeed, MaxSpeed * m_CautiousSpeedFactor, cautiousnessRequired);
			float   accel       = Mathf.Clamp((desiredSpeed - speed) * m_AccelSensitivity, -1, 1);
			
			//Steer Calculation
			Vector3 localTarget = transform.InverseTransformPoint(offsetTargetPos);
			float   targetAngle = Mathf.Atan2(localTarget.x, localTarget.z)            * Mathf.Rad2Deg;
			float   steer       = Mathf.Clamp(targetAngle * m_SteerSensitivity, -1, 1) * Mathf.Sign(speed);
			throttle = accel;
			turn     = steer;
			CollisionStayReset();
		}

		float MiniMumDistance;

		void CheckWaypointDistance()
		{
			var distance                                    = Vector3.Distance(transform.position, Target.position);
			if (MiniMumDistance > distance) MiniMumDistance = distance;
			if (distance > MiniMumDistance || distance < 4f)
			{
				UpdateNode();
			}
		}

		void UpdateNode()
		{
			if (currentNode == nodes.Count - 1)
			{
				currentNode = -1;
				return;
			}

			currentNode++;
			Target          = nodes[currentNode];
			MiniMumDistance = Vector3.Distance(transform.position, Target.position);
		}

		//Sensors
		void CollisionStayReset()
		{
			if (_engine.lastCollisionTime > 3)
			{
				//Reset
				transform.rotation = Quaternion.LookRotation((Target.position - transform.position).normalized, Vector3.up);
			}
		}

		void ResetPosition()
		{
			if (currentNode > 0)
				transform.SetPositionAndRotation(nodes[currentNode - 1].position + Vector3.up * 3, Quaternion.LookRotation((Target.position - transform.position).normalized, Vector3.up));
		}

	}


}