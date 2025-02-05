using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StudySystem
{
	public sealed class ChoiceCard : Card
	{
		[Space(25), Header("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"), Space(25)]
		[SerializeField] private List<Card> Cards = new List<Card>();
		protected override List<Card> _allCards { get => Cards; set => Cards = value; }

		protected override void Continue()
		{
			base.Continue();
			
			IsCompleted = Cards.All((card) => card.IsCompleted);
			if (IsCompleted) { m_previousCard.Invoke(); return; }
			
			CardsWindow.Instance.DisplayCards(
				() => Continue(), 
				updateBranch, 
				Cards.ToArray()
			);
		}

		public override void SkipAll() => Cards.ForEach((card) => card.SkipAll());
	}
}