using System;
using UnityEngine;

public class IThirdPersonController
{

//Parts
	IInput      input      { get; set; }
	ILocomotion locomotion { get; set; }
	ISkill[]    skills     { get; set; }
//Functions

}
//controls inputs functions controls like sensors

public interface IInput
{

	bool GetButtonDown(string buttonName);

	float GetAxis(string axisName);

}

//Controls all movement actions
public interface ILocomotion
{

	IInput input { get; }

	void DoUpdate();

}

public interface IFlyingLocomotion : ILocomotion
{

}

public interface ISkill
{

}

public interface IShooter : ISkill
{

}

public interface IFighter : ISkill
{

}

public interface IMeleeFighter : IFighter
{

}

public interface ICoverShooter : IShooter
{

}

public interface IHealthSystem
{

	float               maximumHealth { get; }
	float               currentHealth { get; }
	bool                isAlive       { get; set; }
	event Action        OnDeath;
	event Action<float> OnTakeDamage;
	event Action<float> OnHealDamage;
	event Action        OnRevive;

	void TakeDamage(float damageAmount);

	void HealDamage(float damageAmount);

	void Revive();

}