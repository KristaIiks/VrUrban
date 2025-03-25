using UnityEngine;
using UnityEngine.Playables;

namespace StudySystem
{
	[RequireComponent(typeof(PlayableDirector))]
	// TODO: refactor is fast solution
	public class Cutscenes : MonoBehaviour
	{
		public static Cutscenes Instance;
		
		[SerializeField] private PlayableDirector Playable;
		
		private void OnValidate()
		{
			Playable ??= GetComponent<PlayableDirector>();
		}
		
		private void Awake()
		{
			Instance = this;
		}
		
		public void RunCutscene(PlayableAsset timeline)
		{
			Playable.time = Playable.duration;
			Playable.Evaluate();
			Playable.Stop();
			
			if (timeline == null) { return; }
			
			Playable.Play(timeline);
		}
	}
}