using System;
using DG.Tweening;
using UMGS;
using UnityEngine;
using UnityEngine.Events;


namespace UAnimationSystem
{


	[UClassHeader("Animation System")]
	public abstract class UAnimation : UBehaviour, IAnimation
	{

		[SerializeField]                             bool       autoPlay   = false;
		[SerializeField]                             int        m_priority = 0;
		[SerializeField]                   protected float      delay      = 0, duration = 1;
		[SerializeField]                   protected Ease       ease       = Ease.Linear;
		[SerializeField] [Range(-1, 1000)] protected int        loops      = 1;
		[SerializeField]                   protected LoopType   loopType   = LoopType.Incremental;
		[SerializeField]                   public    UnityEvent onComplete;
		private                                      Tweener    m_tweener;
		public Tweener tweener
		{
			get
			{
				if (m_tweener == null)
				{
					Init();
				}

				return m_tweener;
			}
		}
		public bool isPlaying { get; protected set; }

		public int priority
		{
			get => m_priority;
			protected set => m_priority = value;
		}


		public virtual void Play()
		{
			isPlaying = true;
			tweener?.PlayForward();
		}

		public virtual void Pause()
		{
			isPlaying = false;
			tweener?.Pause();
		}

		public virtual void Restart()
		{
			tweener?.Restart(false);
		}

		public void Reverse()
		{
			tweener?.PlayBackwards();
		}

		protected virtual void OnDisable()
		{
			tweener?.Kill();
			m_tweener = null;
		}

		protected abstract Tweener CreateAnimation();

		protected virtual void OnEnable()
		{
			if (autoPlay)
			{
				Play();
			}
		}

		public void Init()
		{
			//print($"{gameObject.name}");
			m_tweener?.Kill();
			m_tweener = CreateAnimation();
			m_tweener.SetEase(ease).SetDelay(delay).SetAutoKill(false);
			if (loops != 0 || loops != 1) m_tweener.SetLoops(loops, loopType);
			if (onComplete != null)
				m_tweener.OnComplete(onComplete.Invoke);
		}

		[ContextMenu("Play")]
		public void PlayEditor()
		{
			if (Application.isPlaying)
				Play();
		}

	}

	public abstract class UAnimationFromTo : UAnimation
	{

		[SerializeField] protected Vector3 from, to;

	}

	public interface IAnimation
	{

		Tweener tweener   { get; }
		bool    isPlaying { get; }
		int     priority  { get; }

		void Init();

		void Play();

		void Pause();

		void Restart();

		void Reverse();

	}


}