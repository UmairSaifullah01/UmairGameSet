using System;
using DG.Tweening;
using UnityEngine;


namespace UAnimationSystem
{


	public class ScaleAnimation : UAnimationFromTo
	{

		protected override Tweener CreateAnimation()
		{
			
			return transform.DOScale(to, duration).From(@from);
		}


		[ContextMenu("From")]
		public void SetFrom()
		{
			@from = transform.localScale;
		}

		[ContextMenu("To")]
		public void SetTo()
		{
			to                   = transform.localScale;
			transform.localScale = @from;
		}

		[ContextMenu("Play")]
		public void PlayEditor()
		{
			Play();
		}

	}


}