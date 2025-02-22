using UnityEngine;
using UnityEngine.Playables;

namespace StudySystem
{
	[RequireComponent(typeof(PlayableDirector))]
	public class Cutscenes : MonoBehaviour
	{
		public static Cutscenes Instance;
		
		private PlayableDirector _playable;
		
		private void OnValidate()
		{
			Instance = this;
			_playable ??= GetComponent<PlayableDirector>();
		}
		
		public void RunCutscene(PlayableAsset timeline)
		{
			_playable.Stop();
			
			if (timeline == null) { return; }
				
			_playable.playableAsset = timeline;
			_playable.Play();
		}
	}
}