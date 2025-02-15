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

		public override void StartCard(Action previousCard, Action<Card> updateBranch)
		{
			base.StartCard(previousCard, updateBranch);
			OnComplete?.Invoke();
		}

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

		public override void SkipAll()
		{
			OnStart?.Invoke();
			OnComplete?.Invoke();
			
			Cards.ForEach((card) => card.SkipAll());
		}
		public override void Skip(Card to)
		{
			base.Skip(to);
			to.StartCard(() => Continue(), updateBranch);
		}
	}
}