using System;
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

		public override void StartCard(Action previousCard, Branch branch)
		{
			base.StartCard(previousCard, branch);
			IsCompleted = true;
			OnComplete?.Invoke();
			
			Continue();
		}

		protected override void Continue()
		{			
			if (Cards.All((card) => card.IsCompleted)) { m_previousCard.Invoke(); return; }
			
			CardsWindow.Instance.DisplayCards(
				() => Continue(), 
				m_branch, 
				Cards.ToArray()
			);
		}

		public override void SkipAll()
		{
			OnStart?.Invoke();
			OnComplete?.Invoke();
			
			Cards.ForEach((card) => card.SkipAll());
		}
		public override void Skip(Card to)
		{
			base.Skip(to);
			to.StartCard(() => Continue(), m_branch);
		}
	}
}