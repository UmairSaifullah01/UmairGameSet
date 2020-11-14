using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UMGS
{


	public abstract class StateMachine : MonoBehaviour, IStateMachine
	{

		public             string                     currentStateName;
		protected readonly Dictionary<string, IState> cachedStates = new Dictionary<string, IState>();
		protected          IState                     currentState, previousState;
		protected readonly Stack<IState>              anyStates = new Stack<IState>();
		public             bool                       isTransiting { get; private set; }

		public abstract void LoadState(string id, Action<IState> onStateLoad);


		public IState GetState(string id)
		{
			return cachedStates[id];
		}

		public void Entry(IState state)
		{
			currentState = state;
			currentState?.Enter();
		}

		public void Transition(ITransition transition)
		{
			if (!isTransiting)
			{
				isTransiting = true;
				StartCoroutine(TransitionTo(transition));
			}
		}

		private IEnumerator TransitionTo(ITransition transition)
		{
			yield return new WaitForSecondsRealtime(currentState.exitTime);
			currentState?.Exit();
			previousState = currentState;
			currentState  = GetState(transition.toState);
			currentState?.Enter();
			isTransiting = false;
		}

		public void AnyTransition(IState state)
		{
			anyStates.Push(state);
			state.Enter();
		}

		public void ExitAnyStates()
		{
			if (anyStates.Count > 0)
				anyStates.Pop().Exit();
		}

		public void StatesExecution()
		{
			currentStateName = currentState?.ToString();
			if (isTransiting) return;
			currentState?.Execute();
			foreach (var anyState in anyStates)
			{
				anyState?.Execute();
			}
		}

		public void Exit(IState state)
		{
			state?.Exit();
			if (currentState == state) currentState = null;
		}

	}


}