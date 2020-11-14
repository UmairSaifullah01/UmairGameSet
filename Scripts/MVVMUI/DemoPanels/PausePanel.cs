using UMGS;
using UMUINew;
using UnityEngine;


public class PausePanel : UIPanel
{

	public override void Init(IStateMachine stateMachine)
	{
		base.Init(stateMachine);
		SetTransitions(new Transition(nameof(GamePlayPanel)));
		EventBinder("BTNRetry",  Restart);
		EventBinder("BTNResume", Resume);
	}

	public override void Enter()
	{
		base.Enter();
		Time.timeScale = 0;
	}

	public override void Exit()
	{
		base.Exit();
		Time.timeScale = 1;
	}

	void Resume()
	{
		SetTransitionCondition(nameof(GamePlayPanel), true);
	}

	void Restart()
	{
		SceneLoader.ReLoadScene();
	}

}