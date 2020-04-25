using System;
using System.Collections.Generic;
using UnityEngine;


namespace UMGS.WayPointSystem
{


	public class WayPointManager : MonoBehaviour
	{

		public float          width;
		public List<WayPoint> path = new List<WayPoint>();
		public WayPoint       head, tail;
		public float          distance;
		public WayPoint this[int index]
		{
			get
			{
				if (index > path.Count - 1) return null;
				return path[index];
			}
		}

		void Awake()
		{
			head = transform.GetChild(0).GetComponent<WayPoint>();
			Init();
		}


		public int GetWayPointIndex(WayPoint point)
		{
			return path.IndexOf(point);
		}

		public float DistanceToWaypoint(WayPoint wayPoint)
		{
			float    distance = 0;
			WayPoint wp       = wayPoint;
			while (wp.previousWayPoint)
			{
				distance += Vector3.Distance(wp.GetPosition(), wp.previousWayPoint.GetPosition());
			}

			return distance;
		}

		private void Init()
		{
			WayPoint wayPoint = head;
			while (wayPoint.nextWayPoint)
			{
				path.Add(wayPoint);
				distance += Vector3.Distance(wayPoint.GetPosition(), wayPoint.nextWayPoint.GetPosition());
				wayPoint =  wayPoint.nextWayPoint;
			}

			tail = wayPoint;
			tail = wayPoint;
		}

	}


}