using System;
using System.Collections;
using System.Collections.Generic;
using UMGS;
using UnityEngine;


// ReSharper disable once IdentifierTypo
// ReSharper disable once CheckNamespace
namespace UMUINew
{


	// ReSharper disable once InconsistentNaming
	public class UIHandler : MonoBehaviour, IStateMachine
	{

		readonly Dictionary<string, IState> cachedStates = new Dictionary<string, IState>();
		IState                     currentState, previousState;
		readonly Stack<IState>              anyStates = new Stack<IState>();
		public bool                isTransiting { get; private set; }

		void Awake()
		{
			var states = GetComponentsInChildren<IState>(true);
			foreach (var state in states)
			{
				cachedStates.Add(state.GetType().Name, state);
				state.Init(this);
			}

			Entry(cachedStates["SplashPanel"]);
		}

		void Update()
		{
			StatesExecution();
		}


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
			currentState.Execute();
			foreach (var anyState in anyStates)
			{
				anyState.Execute();
			}
		}

		public void Exit(IState state)
		{
			throw new System.NotImplementedException();
		}

	}


}