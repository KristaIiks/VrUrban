using System;
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
			
			Invoke(nameof(Continue), (float)Timeline.duration);
		}

		protected override void Continue() => m_previousCard.Invoke();

		public override void SkipAll() { }
	}
}
