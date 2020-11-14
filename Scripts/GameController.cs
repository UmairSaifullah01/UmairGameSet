using UMDataManagement;
using UMGS;

public class GameController : SingletonLocal<GameController>
{

	public PlayerController player;
	public EnemyController  enemyController;
	protected override void Awake()
	{
		base.Awake();
		EventHandler.OnGameStart.SetListener(StartGame);
	}

	void StartGame()
	{
		player.Activate();
		enemyController.Activate();
		CameraManager.ActiveCamera("TPSView");
	}

	void OnApplicationQuit()
	{
		DataManager.SaveToFile();
	}

}