using DG.Tweening;
using UnityEngine;


namespace UAnimationSystem
{


	public class LocalMoveAnimation : UAnimationFromTo
	{

		protected override Tweener CreateAnimation()
		{
			return transform.DOLocalMove(to, duration).From(@from);
			
		}


		[ContextMenu("From")]
		public void SetFrom()
		{
			@from = transform.localPosition;
		}

		[ContextMenu("To")]
		public void SetTo()
		{
			to                      = transform.localPosition;
			transform.localPosition = @from;
		}

		

	}


}