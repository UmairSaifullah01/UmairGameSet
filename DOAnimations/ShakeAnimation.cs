using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UAnimationSystem;
using UnityEngine;

public class ShakeAnimation : UAnimation
{

	[SerializeField] float strength   = 1f;
	[SerializeField] int   vibrations = 10;

	protected override Tweener CreateAnimation()
	{
		return transform.DOShakePosition(duration, strength, vibrations);
	}

}