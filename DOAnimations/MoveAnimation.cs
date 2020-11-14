using DG.Tweening;
using UnityEngine;


namespace UAnimationSystem
{


	public class MoveAnimation : UAnimationFromTo
	{

		protected override Tweener CreateAnimation()
		{
			return transform.DOMove(to, duration).From(@from);
		}


		[ContextMenu("From")]
		public void SetFrom()
		{
			@from = transform.position;
		}

		[ContextMenu("To")]
		public void SetTo()
		{
			to                 = transform.position;
			transform.position = @from;
		}

	}


}