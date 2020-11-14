using UMGS;
using UMGS.SoundSystem;


namespace UMUINew
{


	public class CompletePanel : UIPanel
	{

		int currentOffer = 0;

		public override void Init(IStateMachine stateMachine)
		{
			base.Init(stateMachine);
			EventBinder("HomeButton", HomeButton);
			EventBinder("BTNNext",    RestartButton);
		}

		public override void Enter()
		{
			base.Enter();
			// SoundManager.Instance.Play("Success");
		}

		void RestartButton()
		{
			SceneLoader.ReLoadScene();
		}


		void HomeButton()
		{
			EventHandler.OnGameToHome.Send();
		}

	}


}