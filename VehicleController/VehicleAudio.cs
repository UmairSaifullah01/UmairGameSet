using UnityEngine;


namespace UMGS.Vehicle
{


	[System.Serializable]
	public class VehicleAudio
	{

		[Header("Pitch Parameter")] public float       flatoutSpeed = 20.0f;
		[Range(0.0f, 3.0f)]         public float       minPitch     = 0.7f;
		[Range(0.0f, 0.1f)]         public float       pitchSpeed   = 0.05f;
		[Header("Clips")]           public AudioClip   rolling;
		public                             AudioClip   starting, impact, skid;
		[Header("Clips")] public           AudioSource engineSource;
		public                             AudioSource othersSource;
		ControlInput                                   _input;
		bool                                           startUp = false;
		float                                          lastSkid;

		public void Initialize(ControlInput input)
		{
			_input = input;
		}

		public void DoUpdate(float speed)
		{
			if (_input.run && !startUp)
			{
				engineSource.clip = starting;
				engineSource.Play();
				engineSource.pitch = 1;
				startUp            = true;
			}

			if (startUp && !engineSource.isPlaying)
			{
				engineSource.clip = rolling;
				engineSource.loop = true;
				engineSource.Play();
			}

			if (engineSource.clip == rolling)
			{
				engineSource.pitch = Mathf.Clamp(Mathf.Lerp(engineSource.pitch, minPitch + Mathf.Abs(speed) / flatoutSpeed, pitchSpeed), minPitch, 2);
			}
		}

		public void ImpactAudio(Vector3 atPoint)
		{
			PlayOneTime(impact, atPoint, 0.5f, 1);
		}

		public void SkidAudio(Vector3 atPoint, float volume)
		{
			if (Time.time - lastSkid > 2f)
				PlayOneTime(skid, atPoint, volume * 0.2f, 1);
			lastSkid = Time.time;
		}

		void PlayOneTime(AudioClip clip, Vector3 position, float volume, float pitch)
		{
			if (pitch < 0.01f || volume < 0.01f) return;
			AudioSource source = Object.Instantiate(othersSource, position, Quaternion.identity, othersSource.transform.parent);
			if (source.isActiveAndEnabled)
			{
				source.clip         = clip;
				source.volume       = volume;
				source.pitch        = pitch;
				source.dopplerLevel = 0.0f; // Doppler causes artifacts as for positioning the audio source
				source.Play();
			}

			Object.Destroy(source.gameObject, clip.length / pitch);
		}

	}


}