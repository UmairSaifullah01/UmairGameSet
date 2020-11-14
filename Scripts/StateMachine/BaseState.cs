using System.Collections.Generic;
using System.Linq;
using UMGS;

public abstract class BaseState : IState
{

	public float         exitTime     { get; protected set; }
	public IStateMachine stateMachine { get; protected set; }
	List<ITransition>    cachedTransitions = new List<ITransition>();

	public virtual void Init(IStateMachine stateMachine)
	{
		this.stateMachine = stateMachine;
	}

	public void SetTransitions(params ITransition[] transitions)
	{
		foreach (var transition in transitions)
		{
			stateMachine.LoadState(transition.toState, null);
		}

		this.cachedTransitions = transitions.ToList();
	}

	public ITransition[] GetTransitions()
	{
		return cachedTransitions.ToArray();
	}

	public void SetTransitionCondition(string stateName, bool value)
	{
		var trans                          = cachedTransitions.FirstOrDefault(x => x.toState == stateName);
		if (trans != null) trans.condition = value;
	}

	public virtual void Execute()
	{
		var executableTransition = cachedTransitions.FirstOrDefault(x => x.condition);
		if (executableTransition != null)
		{
			stateMachine.Transition(executableTransition);
		}
	}

	public abstract void Enter();

	public abstract void Exit();

}