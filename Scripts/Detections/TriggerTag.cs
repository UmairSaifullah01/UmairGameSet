﻿using System;
using UMGS;
using UnityEngine;
using UnityEngine.Events;

public class TriggerTag : UJTrigger
{

	public string     TargetTag;
	public UnityEvent OnTriggerEnter;

	void Start()
	{
		enter += Enter;
	}

	void Enter(Collider col)
	{
		if (col.attachedRigidbody.CompareTag(TargetTag)) OnTriggerEnter?.Invoke();
	}

}