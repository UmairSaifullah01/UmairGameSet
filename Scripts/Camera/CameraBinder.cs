using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBinder : MonoBehaviour, ICameraBinder
{

	public string Id => gameObject.name;

	public Transform GetTransform() => transform;

	Camera cam;

	public void Activate()
	{
		gameObject.SetActive(true);
	}

	public void Deactivate()
	{
		gameObject.SetActive(false);
	}


	public Camera GetCamera()
	{
		return cam ? cam : Camera.main;
	}

	public virtual void Init()
	{
		cam = GetComponent<Camera>();
		Deactivate();
	}

}

public interface ICameraBinder
{

	string Id { get; }

	void Init();

	Transform GetTransform();

	void Activate();

	void Deactivate();

	Camera GetCamera();

}