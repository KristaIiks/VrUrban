using System;
using TMPro;
using UnityEngine;

namespace StudySystem
{
	public sealed class RewardCardView : DefaultCardView
	{
		[field:Space(25)]
		[SerializeField] private TMP_Text RewardText;
		[SerializeField] private TMP_Text PriceText;
		
		public override void Init(Action cardAction, Action disableWindow, CardSO info)
		{
			base.Init(cardAction, disableWindow, info);
			
			if (info is not RewardQuestCardSO questCard) { return; }
			
			RewardText.text = $"Комфортность: {questCard.Stats.Comfort}\n" + 
				$"Экология: {questCard.Stats.Ecology}\n" +
				$"Безопасность: {questCard.Stats.Security}\n" +
				$"Стоимость жилья: {questCard.Stats.HousePrice}\n" +
				$"Эстетика: {questCard.Stats.Beauty}\n";
			
			PriceText.text = questCard.Price.ToString();
		}
		
		public override Type GetCardsType() => typeof(RewardQuestCardSO);
	}
}
