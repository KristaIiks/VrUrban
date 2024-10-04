using UnityEngine;
using UnityEngine.Audio;

namespace Extensions.Audio
{
	[System.Serializable]
	public struct StartupAudioSettings
	{
		public AudioMixerGroup audioGroup;
		[Range(0f, 1f)] public float? volume;
		public float? pitch;
		
		/// <summary>
		/// Generate only custom range
		/// </summary>
		/// <param name="range"></param>
		public StartupAudioSettings(Vector2 range)
		{
			audioGroup = null;
			volume = null;
			
			pitch = Random.Range(range.x, range.y);
		}
	}
}