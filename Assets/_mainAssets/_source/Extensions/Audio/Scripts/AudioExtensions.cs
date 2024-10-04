using UnityEngine;

namespace Extensions.Audio
{
	public static class AudioExtensions
	{
		/// <summary>
		/// Play One shot audio with override settings
		/// </summary>
		/// <param name="source"></param>
		/// <param name="clip"></param>
		/// <param name="settings"></param>
		public static void PlayOneShot(this AudioSource source, AudioClip clip, StartupAudioSettings settings)
		{
			AudioSource _newSource = CreateAndAssign(source, clip, settings);
			_newSource.Play();
			
			Object.Destroy(_newSource, clip.length);
		}
		
		/// <summary>
		/// Play audio with randomized pitch in custom range
		/// </summary>
		/// <param name="source"></param>
		/// <param name="clip"></param>
		/// <param name="pitchRange">Min, max value</param>
		public static void PlayRandomized(this AudioSource source, AudioClip clip, Vector2 pitchRange)
		{
			StartupAudioSettings settings = new StartupAudioSettings(pitchRange);
			PlayOneShot(source, clip, settings);
		}
		public static void PlayRandomized(this AudioSource source, AudioClip clip) => PlayRandomized(source, clip, new Vector2(-3, 3));

		/// <summary>
		/// Create new object, add audio and set custom or default params
		/// </summary>
		/// <param name="source"></param>
		/// <param name="clip"></param>
		/// <param name="settings"></param>
		/// <returns></returns>
		private static AudioSource CreateAndAssign(AudioSource source, AudioClip clip, StartupAudioSettings settings)
		{
			AudioSource _newAudioSource = GameObject.Instantiate(new GameObject("[Audio System] OneShot Audio"), source.transform).AddComponent<AudioSource>();
			
			_newAudioSource.outputAudioMixerGroup = settings.audioGroup ?? source.outputAudioMixerGroup;
			_newAudioSource.volume = settings.volume ?? source.volume;
			_newAudioSource.pitch = settings.pitch ?? source.pitch;
			_newAudioSource.clip = clip;
			
			return _newAudioSource;
		}
	}
}