using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace StudySystem
{
	public class PlayableCard : Card
	{
		[Space(25)]
		[SerializeField] private TimelineAsset Timeline;
		
		protected override List<Card> _allCards { get; set; } = new List<Card>();

		public override void StartCard(Action previousCard, Branch branch)
		{
			base.StartCard(previousCard, branch);
			Cutscenes.Instance.RunCutscene(Timeline);
			
			StartCoroutine(Continue((float)Timeline.duration));
		}

		protected override void Continue()
		{
			IsCompleted = true;
			OnComplete?.Invoke();
			
			m_previousCard.Invoke();
		}
		private IEnumerator Continue(float time)
		{
			yield return new WaitForSeconds(time);
			Continue();
		}

		public override void SkipAll()
		{
			Cutscenes.Instance.RunCutscene(Timeline);
			
			IsCompleted = true;
			OnComplete?.Invoke();
		}
	}
}
