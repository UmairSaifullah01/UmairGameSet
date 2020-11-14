using System.Linq;
using UMGS;


namespace UMUINew
{


	public class FailPanel : UIPanel
	{

		public override void Init(IStateMachine stateMachine)
		{
			base.Init(stateMachine);
			EventBinder("BTNRetry", Restart);
		}


		void Restart()
		{
			SceneLoader.ReLoadScene();
		}

	}


}