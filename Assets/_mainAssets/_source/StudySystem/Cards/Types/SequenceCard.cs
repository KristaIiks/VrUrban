using System;
using System.Collections.Generic;
using UnityEngine;

namespace StudySystem
{
	public sealed class SequenceCard : Card
	{
		[Space(25), Header("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"), Space(25)]
		[SerializeField] private List<Card> CardsOrder = new List<Card>();
		protected override List<Card> _allCards { get => CardsOrder; set => CardsOrder = value; }

		public override void StartCard(Action previousCard, Branch branch)
		{
			base.StartCard(previousCard, branch);
			IsCompleted = true;
			OnComplete?.Invoke();
			
			Continue();
		}

		public override void SkipAll()
		{
			OnStart?.Invoke();
			CardsOrder.ForEach(card => card.SkipAll());
			IsCompleted = true;
			OnComplete?.Invoke();
		}
		public override void Skip(Card to)
		{
			base.Skip(to);
			
			foreach (Card card in CardsOrder)
			{
				if (card == to) { break; }
				card.SkipAll();
			}
			
			to.StartCard(() => Continue(), m_branch);
		}

		protected override void Continue()
		{			
			foreach (Card card in CardsOrder)
			{
				if (!card.IsCompleted)
				{
					if (card.Info != null)
					{
						CardsWindow.Instance.DisplayCards(
							() => Continue(), 
							m_branch, 
							new Card[] { card }
						); 
					}
					else
					{
						card.StartCard(() => Continue(), m_branch);
					}
					
					return;
				}
			}
			
			m_previousCard.Invoke();
		}
	}
}