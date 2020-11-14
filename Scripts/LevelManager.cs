
using UMDataManagement;
using UMGS;
using UnityEngine;

public class LevelManager : SingletonLocal<LevelManager>
{

	[SerializeField] Level[] levels;
	public           ILevel  currentLevel;

	protected override void Awake()
	{
		base.Awake();
		foreach (Level level in levels)
		{
			if (level.isCompleted) continue;
			currentLevel = level;
			break;
		}

		if (currentLevel == null) currentLevel = levels[0];
		currentLevel.Initialize();
	}

	public void CompleteLevel()
	{
		currentLevel.Complete();
	}

	public void NextLevel()
	{
		if (currentLevel.nextLevel == LevelType.NormalLevel)
		{
			this.AfterWait(() => SceneLoader.LoadSceneAsync("fisher theme"), 1f);
		}
		else
		{
			this.AfterWait(() => SceneLoader.LoadSceneAsync("DroneScene"), 1f);
		}
	}

	public void LevelFail()
	{
		
	}

}


public abstract class Level : MonoBehaviour, ILevel
{

	[SerializeField] int       m_LevelNo;
	[SerializeField] LevelType m_NextLevel;
	public           int       levelNo     => m_LevelNo;
	public           LevelType nextLevel   => m_NextLevel;
	public           bool      isCompleted => DataManager.Get<int>(levelName) == 1;

	public string levelName => $"Level {levelNo}";

	public abstract void Initialize();


	public abstract void Complete();

}

public interface ILevel
{

	int       levelNo     { get; }
	string    levelName   { get; }
	LevelType nextLevel   { get; }
	bool      isCompleted { get; }

	void Initialize();

	void Complete();

}

public enum LevelType
{

	NormalLevel,
	DroneLevel

};