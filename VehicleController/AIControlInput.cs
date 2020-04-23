using System;
using System.Collections.Generic;
using PathCreation;
using UMGS.WayPointSystem;
using UnityEngine;


namespace UMGS.Vehicle
{


	[RequireComponent(typeof(VehicleEngine))]
	public class AIControlInput : ControlInput
	{

		[SerializeField]                         WayPointManager path;
		[SerializeField] [Range(0, 1)]   private float           m_CautiousSpeedFactor           = 0.05f; // percentage of max speed to use when being maximally cautious
		[SerializeField] [Range(0, 180)] private float           m_CautiousMaxAngle              = 50f;
		[SerializeField]                 private float           m_SteerSensitivity              = 0.05f;
		[SerializeField]                 private float           m_AccelSensitivity              = 0.04f;
		[SerializeField]                 private float           m_CautiousAngularVelocityFactor = 30f;

		[SerializeField]                 float    MaxSpeed;
		[SerializeField] [Range(0f, 1f)] float    OffsetX;
		[SerializeField]                 Sensor[] Sensors;
		public                           WayPoint currentWayPoint;
		VehicleEngine                             _engine;
		Rigidbody                                 _rigidbody => _engine.attachedRigidbody;
		private Vector3                           offsetTargetPos;


		void Start()
		{
			_engine         = GetComponent<VehicleEngine>();
			currentWayPoint = path.GetWayPoint(0);
			offsetTargetPos = currentWayPoint.GetPosition(OffsetX);
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
			if (currentWayPoint.nextWayPoint == null)
			{
				Stop();
				return;
			}

			CheckWaypointDistance();
			Vector3 fwd = transform.forward;
			//Accelaration Calculations
			float desiredSpeed           = MaxSpeed;
			float approachingCornerAngle = Vector3.Angle(currentWayPoint.transform.forward, fwd);
			float spinningAngle          = _rigidbody.angularVelocity.magnitude * m_CautiousAngularVelocityFactor;
			float cautiousnessRequired   = Mathf.InverseLerp(0, m_CautiousMaxAngle, Mathf.Max(spinningAngle, approachingCornerAngle));
			desiredSpeed = Mathf.Lerp(MaxSpeed, MaxSpeed * m_CautiousSpeedFactor, cautiousnessRequired);
			float accel = Mathf.Clamp((desiredSpeed - speed) * m_AccelSensitivity, -1, 1);

			//Steer Calculation
			Vector3 localTarget = transform.InverseTransformPoint(offsetTargetPos);
			float   targetAngle = Mathf.Atan2(localTarget.x, localTarget.z)            * Mathf.Rad2Deg;
			float   steer       = Mathf.Clamp(targetAngle * m_SteerSensitivity, -1, 1) * Mathf.Sign(speed);

			//Assigning result values
			throttle = accel;
			turn     = Mathf.Lerp(turn, steer, Time.deltaTime);
			foreach (Sensor sensor in Sensors)
			{
				sensor.Evaluate();
			}

			//CollisionStayReset();
		}

		float MiniMumDistance;

		void CheckWaypointDistance()
		{
			var distance                                    = Vector3.Distance(transform.position, offsetTargetPos);
			if (MiniMumDistance > distance) MiniMumDistance = distance;
			if (distance < 4f)
			{
				UpdateNode();
			}
		}

		void UpdateNode()
		{
			if (currentWayPoint.nextWayPoint)
				currentWayPoint = currentWayPoint.nextWayPoint;
			offsetTargetPos = currentWayPoint.GetPosition(OffsetX);
			MiniMumDistance = Vector3.Distance(transform.position, offsetTargetPos);
		}

		//Sensors
		void CollisionStayReset()
		{
			if (_engine.lastCollisionTime > 3)
			{
				//Reset
				transform.rotation = Quaternion.LookRotation((offsetTargetPos - transform.position).normalized, Vector3.up);
			}
		}

		void ResetPosition()
		{
			transform.SetPositionAndRotation(offsetTargetPos + Vector3.up * 3, Quaternion.LookRotation((offsetTargetPos - transform.position).normalized, Vector3.up));
		}

	}


}