using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StudySystem
{
	public class BasicCard : QuestCard
	{
		[Space(25), Header("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"), Space(25)]
		[SerializeField] private List<Card> Cards = new List<Card>();
		protected override List<Card> _allCards { get => Cards; set => Cards = value; }
		
		protected override void Continue()
		{
			base.Continue();
			
			if (Cards.Count == 0 || Cards.All((card) => card.IsCompleted))
			{
				IsCompleted = true;
				
				m_previousCard?.Invoke();
				return;
			}
			
			CardsWindow.Instance.DisplayCards(() => Continue(), updateBranch, Cards.ToArray());
		}

		public override void SkipAll()
		{
			OnStart?.Invoke();
			Quests.ForEach((quest) => quest.Skip());
			OnComplete?.Invoke();
			
			Cards.ForEach((card) => card.SkipAll());
		}
	}
}