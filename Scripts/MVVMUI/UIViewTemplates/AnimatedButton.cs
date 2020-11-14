using DG.Tweening;
using UAnimationSystem;


namespace UMUINew
{


	public class AnimatedButton : AdvanceButtonBinder
	{

		public UAnimation animation;

		protected override void Press()
		{
			if (!IsActive())
				return;
			
			animation.Play();
		}

		protected override void Unpress()
		{
			if (!IsActive())
				return;
			animation.Reverse();
		}

	}


}