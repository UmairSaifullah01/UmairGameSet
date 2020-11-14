using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;


namespace UAnimationSystem
{


	public class AnimationManager : MonoBehaviour
	{

		[SerializeField]                             bool       autoPlay = false;
		[SerializeField] [Range(-1, 1000)] protected int        loops    = 1;
		[SerializeField]                   public    UnityEvent onComplete;
		public                                       Sequence   sequence { get; protected set; }

		void OnEnable()
		{
			CreateGroup();
			if (!autoPlay)
				sequence.Pause();
			sequence.SetLoops(loops);
			if (onComplete != null)
				sequence.OnComplete(onComplete.Invoke);
		}

		void CreateGroup()
		{
			sequence = DOTween.Sequence();
			var animations = GetComponentsInChildren<IAnimation>();
			animations = animations.OrderBy(x => x.priority).ToArray();
			int priority = 0;
			foreach (IAnimation animation1 in animations)
			{
				animation1.Init();
				if (priority == animation1.priority)
				{
					sequence.Join(animation1.tweener);
				}
				else
				{
					priority = animation1.priority;
					sequence.Append(animation1.tweener);
				}
			}
		}

		public void Play()
		{
			sequence.Play();
		}

		public void Pause()
		{
			sequence.Pause();
		}

		public void Restart()
		{
			sequence.Restart(false);
		}

	}


}