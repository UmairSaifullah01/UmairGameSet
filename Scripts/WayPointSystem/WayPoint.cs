using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UMGS.WayPointSystem
{


	public class WayPoint : MonoBehaviour
	{

		public                 WayPoint previousWayPoint;
		public                 WayPoint nextWayPoint;
		[Range(0.1f, 15f)] public float width = 1;

		public Vector3 GetPosition()
		{
			return GetPosition(Random.Range(0f, 1f));
		}

		public Vector3 GetPosition(float deltaOffSet)
		{
			Vector3 minBound = transform.position + transform.right * width / 2;
			Vector3 maxBound = transform.position - transform.right * width / 2;
			return Vector3.Lerp(minBound, maxBound, deltaOffSet);
		}

	}


}