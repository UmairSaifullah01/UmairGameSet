using System;
using UMGS;
using EventHandler = UMGS.EventHandler;


namespace UMUINew
{


	public class GamePlayPanel : UIPanel
	{

		public override void Init(IStateMachine stateMachine)
		{
			base.Init(stateMachine);
			// setting transitions for flow
			var pause    = new Transition(nameof(PausePanel));
			var complete = new Transition(nameof(CompletePanel));
			var fail     = new Transition(nameof(FailPanel));
			SetTransitions(pause, complete, fail);
			EventBinder("BTNPlay",     PlayClick);
			EventBinder("BTNPauseUp",  PauseGame);
			EventBinder("BTNStopDown", () => GameController.Instance.player.playerData.isRunning = false);
			EventBinder("BTNStopUp",   () => GameController.Instance.player.playerData.isRunning = true);
			EventHandler.LevelFail.SetListener(() => SetTransitionCondition(nameof(FailPanel),         true));
			EventHandler.LevelComplete.SetListener(() => SetTransitionCondition(nameof(CompletePanel), true));
			EventHandler.OnGameStart.AddListener(()=> views["Hold"].Active(false));
		}

		void FixedUpdate()
		{
		//	FloatBinder("StaminaBar", GameController.Instance.player.playerData.stamina / 5);
		}

		void PauseGame()
		{
			SetTransitionCondition(nameof(PausePanel), true);
		}

		void PlayClick()
		{
			EventHandler.OnGameStart.Send();
			views["BTNPlay"].Active(false);
			//views["BTNStop"].Active(true);
		}

	}


}