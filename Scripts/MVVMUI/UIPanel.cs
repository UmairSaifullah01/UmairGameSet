using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UMGS;
using UMUI;
using UnityEngine;


namespace UMUINew
{


	// ReSharper disable once InconsistentNaming
	public abstract class UIPanel : MonoBehaviour, IState, IViewModel
	{

		Dictionary<string, ITransition>     cachedTransitions = new Dictionary<string, ITransition>();
		public event Action<string, string> stringBinder;
		public event Action<string, float>  floatBinder;
		public event Action<string, Action> eventBinder;
		public Dictionary<string, IView>    views          { get; set; }
		public IStorageService              storageService { get; set; }


		public float         exitTime     { get; protected set; }
		public IStateMachine stateMachine { get; private set; }


		public virtual void Init(IStateMachine stateMachine)
		{
			storageService    = LocalStorageService.GetService();
			this.stateMachine = stateMachine;
			InitViewModel();
			gameObject.SetActive(false);
		}

		public void SetTransitions(params ITransition[] transitions)
		{
			foreach (var transition in transitions)
			{
				stateMachine.LoadState(transition.toState, null);
				this.cachedTransitions.Add(transition.toState, transition);
			}
		}

		public ITransition[] GetTransitions()
		{
			var transitions = new ITransition[cachedTransitions.Count];
			int iterator    = 0;
			foreach (var transition in cachedTransitions)
			{
				transitions[iterator] = transition.Value;
				iterator++;
			}

			return transitions;
		}

		public void SetTransitionCondition(string stateName, bool value)
		{
			this.cachedTransitions[stateName].condition = value;
		}

		public virtual void Execute()
		{
			var executableTransition = cachedTransitions.FirstOrDefault(x => x.Value.condition);
			if (!string.IsNullOrEmpty(executableTransition.Key))
			{
				stateMachine.Transition(executableTransition.Value);
			}
		}

		public virtual void Enter()
		{
			gameObject.SetActive(true);
			transform.SetAsLastSibling();
		}

		public virtual void Exit()
		{
			gameObject.SetActive(false);
			foreach (var transition in cachedTransitions)
			{
				transition.Value.condition = false;
			}
		}

		public virtual void InitViewModel()
		{
			if (views == null)
			{
				var v = GetComponentsInChildren<IView>(true);
				views = new Dictionary<string, IView>();
				foreach (IView view in v)
				{
					views.Add(view.Id, view);
				}
			}

			foreach (var view in views)
			{
				view.Value.Init(this);
			}
		}


		protected void StringBinder(string Id, string value)
		{
			stringBinder?.Invoke(Id, value);
		}

		protected void FloatBinder(string Id, float value)
		{
			floatBinder?.Invoke(Id, value);
		}

		protected void EventBinder(string Id, Action value)
		{
			eventBinder?.Invoke(Id, value);
		}

	}


}