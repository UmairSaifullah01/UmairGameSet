using UMDataManagement;
using UnityEngine;


public class NormalLevel : Level
{

	[SerializeField] GameObject levelObject;

	public override void Initialize()
	{
		levelObject.SetActive(true);
	}

	public override void Complete()
	{
		DataManager.Save(levelName, 1);
	}

}