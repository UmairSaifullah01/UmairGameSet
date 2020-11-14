using DG.Tweening;
using UnityEngine;


namespace UAnimationSystem
{


	public class LocalRotateAnimation : UAnimationFromTo
	{

		protected override Tweener CreateAnimation()
		{
			return transform.DOLocalRotate(to, duration).From(@from);
		}


		[ContextMenu("From")]
		public void SetFrom()
		{
			@from = transform.localEulerAngles;
		}

		[ContextMenu("To")]
		public void SetTo()
		{
			to                         = transform.localEulerAngles;
			transform.localEulerAngles = @from;
		}

	}


}