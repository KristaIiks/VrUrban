using System.Collections.Generic;
using UnityEngine;

namespace StudySystem
{
	public sealed class SequenceCard : Card
	{
		[Space(25), Header("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"), Space(25)]
		[SerializeField] private List<Card> CardsOrder = new List<Card>();
		protected override List<Card> _allCards { get => CardsOrder; set => CardsOrder = value; }

		public override void SkipAll()
		{
			CardsOrder.ForEach(card => card.SkipAll());
		}
		public override void Skip(Card to)
		{
			base.Skip(to);
			
			foreach (Card card in CardsOrder)
			{
				if (card == to) { return; }
				card.SkipAll();
			}
		}

		protected override void Continue()
		{
			base.Continue();
			
			foreach (Card card in CardsOrder)
			{
				if (!card.IsCompleted)
				{
					if (card.Info != null)
					{
						CardsWindow.Instance.DisplayCards(
							() => Continue(), 
							updateBranch, 
							new Card[] { card }
						); 
					}
					else
					{
						card.StartCard(() => Continue(), updateBranch);
					}
					
					return; 
				}
			}
			
			m_previousCard.Invoke();
		}
	}
}