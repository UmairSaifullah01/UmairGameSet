using UMGS.WayPointSystem;
using UnityEngine;

public class PositionStatus : MonoBehaviour, IPositionStats
{

	private          WayPoint        currentWayPoint;
	[SerializeField] WayPointManager Manager;

	void Start()
	{
		currentWayPoint = Manager.head;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		CheckWaypointDistance();
	}

	public int Id         { get; set; }
	public int PositionNo { get; set; }
	public float CoveredDistance
	{
		get => CoveredDistanceCal();
		set { ; }
	}
	float waypointdistance = 0;

	void CheckWaypointDistance()
	{
		Manager.path.Sort((a, b) => Vector3.Distance(transform.position, a.GetPosition()).CompareTo(Vector3.Distance(transform.position, b.GetPosition())));
		currentWayPoint  = Manager.path[0];
		waypointdistance = Manager.DistanceToWaypoint(currentWayPoint);
	}

	float CoveredDistanceCal()
	{
		float distance = waypointdistance;
		// if (currentWayPoint.previousWayPoint)
		// {
		// 	float c = Vector3.Distance(transform.position, currentWayPoint.GetPosition());
		// 	// float p  = Vector3.Distance(transform.position,            currentWayPoint.previousWayPoint.GetPosition());
		// 	float cp = Vector3.Distance(currentWayPoint.GetPosition(), currentWayPoint.previousWayPoint.GetPosition());
		// 	// float n  = Vector3.Distance(transform.position,            currentWayPoint.nextWayPoint.GetPosition());
		// 	//float cn = Vector3.Distance(currentWayPoint.GetPosition(), currentWayPoint.nextWayPoint.GetPosition());
		// 	distance += cp - c;
		// }
		return distance;
	}

}