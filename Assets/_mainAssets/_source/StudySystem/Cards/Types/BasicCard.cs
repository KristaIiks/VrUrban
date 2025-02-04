using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StudySystem
{
	public class BasicCard : QuestCard
	{
		[Space(25), Header("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"), Space(25)]
		[SerializeField] private List<Card> Cards;
		protected override List<Card> _allCards { get => Cards; set => Cards = value; }
		
		protected override void Continue()
		{
			base.Continue();
			
			if (Cards.All((card) => card.IsCompleted))
			{
				m_previousCard?.Invoke();
				return;
			}
			
			CardsWindow.Instance.DisplayCards(() => Continue(), updateBranch, Cards.ToArray());
		}

		public override void SkipAll()
		{
			Quests.ForEach((quest) => quest.Skip());
			Cards.ForEach((card) => card.SkipAll());
		}
	}
}