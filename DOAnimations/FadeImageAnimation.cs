using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UAnimationSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeImageAnimation : UAnimation
{

	[SerializeField] float endValue;
	Image                  graphic;

	protected override Tweener CreateAnimation()
	{
		graphic   = GetComponent<Image>();
		return graphic.DOFade(endValue, duration);
	}

}