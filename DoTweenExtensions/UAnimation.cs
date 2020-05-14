using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace UMGS.DoTween
{


	public class UAnimation : MonoBehaviour
	{

		[SerializeField] CombineAnim[] AnimElements;

		void OnEnable()
		{
			foreach (CombineAnim anim in AnimElements)
			{
				anim.Play();
			}
		}

	}

	[System.Serializable]
	public abstract class Anim
	{

		public Transform target;
		public float     duration;
		public Ease      ease = Ease.Linear;
		public bool      loop = false;

		public abstract void Play();

		public virtual void Kill()
		{
			target.DOKill();
		}

	}

	[System.Serializable]
	public class RotateAnim : Anim
	{

		[Header("Rotation")] public Vector3 to;

		public Vector3 from;


		public override void Play()
		{
			target.DORotate(to, duration).From(from).SetEase(ease).SetLoops(loop ? -1 : 1);
		}

	}

	[System.Serializable]
	public class ScaleAnim : Anim
	{

		[Header("Scale")] public Vector3 to   = Vector3.one;
		public                   Vector3 from = Vector3.one;


		public override void Play()
		{
			target.DOScale(to, duration).From(from).SetEase(ease).SetLoops(loop ? -1 : 1);
		}

	}

	[System.Serializable]
	public class MoveAnim : Anim
	{

		[Header("Movement")] public Vector3 to;
		public                      Vector3 from;


		public override void Play()
		{
			target.DOMove(to, duration).From(from).SetEase(ease).SetLoops(loop ? -1 : 1);
		}

		public override void Kill()
		{
			target.DOKill();
		}

	}

	[System.Serializable]
	public class CombineAnim
	{

		public MoveAnim  movement;
		public ScaleAnim scale;

		public void Play()
		{
			movement.Play();
			scale.Play();
		}

		public void Kill()
		{
			movement.Kill();
			scale.Kill();
		}

	}


}