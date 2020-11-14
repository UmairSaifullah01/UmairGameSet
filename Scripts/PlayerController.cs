using UnityEngine;

public abstract class PlayerController : Effector
{

	public          PlayerData playerData;
	public abstract void       Activate();

}

[System.Serializable]
public struct PlayerData
{

	public float stamina;
	public bool  isRunning;

}


