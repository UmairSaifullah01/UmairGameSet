using System;
using UnityEngine;

[Serializable]
public class Agent
{

	public                   float   speed            = 3.5f;
	public                   float   angularSpeed     = 3.5f;
	[Range(0.1f, 1)] public  float   acceleration     = 0.8f;
	public                   float   stoppingDistance = 0.01f;
	[HideInInspector] public float   remainingDistance;
	[HideInInspector] public bool    hasPath = false;
	public event Action              onDestinationReach;
	[HideInInspector] public Vector3 velocity;
	[HideInInspector] public Vector3 nextPosition;
	bool                             isPause      = false;
	float                            currentSpeed = 0.0f;

	public virtual void Resume() => isPause = false;

	public virtual void Pause() => isPause = true;

	public virtual void SetDestination(Vector3 target)
	{
		nextPosition = target;
		hasPath      = true;
	}

	public virtual void Move(ref Vector3 currentPosition, ref Quaternion currentRotation, float deltaTime)
	{
		if (!hasPath || isPause)
			return;
		var   direction = (nextPosition - currentPosition).normalized;
		float t         = speed / (speed * acceleration);
		currentSpeed      = Mathf.Lerp(currentSpeed, speed, deltaTime / t);
		velocity          = direction * currentSpeed;
		remainingDistance = Vector3.Distance(nextPosition, currentPosition);
		if (remainingDistance <= stoppingDistance)
		{
			Reach();
		}

		currentPosition += velocity;
		currentRotation =  Quaternion.Slerp(currentRotation, Quaternion.LookRotation(direction), angularSpeed * deltaTime);
	}

	protected virtual void Reach()
	{
		hasPath      = false;
		currentSpeed = 0;
		onDestinationReach?.Invoke();
	}

}