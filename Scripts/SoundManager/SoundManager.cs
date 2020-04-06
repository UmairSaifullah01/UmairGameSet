using UMGS;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : SingletonLocal<SoundManager>
{

	AudioSource source;
	
	public void PlaySoundAtLocation(string name, Vector3 location)
	{
		
	}
}