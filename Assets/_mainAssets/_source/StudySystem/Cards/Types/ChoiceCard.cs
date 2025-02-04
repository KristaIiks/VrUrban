using System.Collections.Generic;
using UnityEngine;

namespace StudySystem
{
	public sealed class ChoiceCard : Card
	{
		[Space(25), Header("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"), Space(25)]
		[SerializeField] private List<Card> Cards = new List<Card>();
		protected override List<Card> _allCards { get => Cards; set => Cards = value; }

		public override void SkipAll() => Cards.ForEach((card) => card.SkipAll());
	}
}