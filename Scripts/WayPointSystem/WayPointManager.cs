using System;
using System.Collections.Generic;
using UnityEngine;


namespace UMGS.WayPointSystem
{


	public class WayPointManager : MonoBehaviour
	{

		public float      width;
		public WayPoint[] path;

		void Awake()
		{
			path = GetComponentsInChildren<WayPoint>();
		}

		public WayPoint GetWayPoint(int index)
		{
			if (index > path.Length - 1) return null;
			return path[index];
		}

	}


}