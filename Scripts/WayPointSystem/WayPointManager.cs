﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


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
			if (transform.childCount == 0)
			{
				Destroy(GetComponent<WayPointManager>());
				return;
			}

			Init();
		}

		public WayPoint GetRandomWayPoint()
		{
			return path[Random.Range(0, path.Count)];
		}

		public WayPoint[] CopyPoints(int start, int count)
		{
			var wayPoints = new WayPoint[count];
			path.CopyTo(start, wayPoints, 0, count);
			return wayPoints;
		}

		public int GetWayPointIndex(WayPoint point)
		{
			return path.IndexOf(point);
		}

		public float GetDistance(WayPoint wayPoint)
		{
			float    distance = 0;
			WayPoint wp       = wayPoint;
			while (wp.previousWayPoint)
			{
				distance += wayPoint.distanceFromPrevious;
				wp       =  wayPoint.previousWayPoint;
			}

			return distance;
		}

		public float GetDistance(WayPoint wayPoint, int count)
		{
			float    distance = 0;
			WayPoint wp       = wayPoint;
			int      counter  = 0;
			while (wp.previousWayPoint && count > counter)
			{
				distance += wayPoint.distanceFromPrevious;
				wp       =  wayPoint.previousWayPoint;
				counter++;
			}

			return distance;
		}

		private void Init()
		{
			head = transform.GetChild(0).GetComponent<WayPoint>();
			WayPoint wayPoint = head;
			if (!wayPoint) return;
			while (wayPoint.nextWayPoint != null && !path.Contains(wayPoint.nextWayPoint))
			{
				path.Add(wayPoint);
				wayPoint.distanceFromPrevious =  wayPoint.previousWayPoint ? Vector3.Distance(wayPoint.GetPosition(), wayPoint.previousWayPoint.GetPosition()) : 0;
				wayPoint.distanceToNext       =  Vector3.Distance(wayPoint.GetPosition(), wayPoint.nextWayPoint.GetPosition());
				distance                      += wayPoint.distanceToNext;
				wayPoint                      =  wayPoint.nextWayPoint;
			}

			tail = wayPoint;
		}

	}


}