using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StudySystem
{
	public sealed class EntryCardView : CardView
	{
		[SerializeField] private TMP_Text NameText;
		[SerializeField] private TMP_Text DescriptionText;
		[SerializeField] private Image Image;
		[SerializeField] private Button Button;
		
		[field:Space(25)]
		[SerializeField] private TMP_Text MeterPriceText;
		[SerializeField] private TMP_Text ComfortText;
		[SerializeField] private TMP_Text EcoText;
		[SerializeField] private TMP_Text SecurityText;
		[SerializeField] private TMP_Text HousePriceText;
		[SerializeField] private TMP_Text BeautyText;
		
		public override void Init(Action cardAction, Action disableWindow, CardSO info)
		{
			if (info is not EntryCardSO entryCard)
				return;
			
			NameText.text = entryCard.Name;
			DescriptionText.text = entryCard.Description;
			Image.sprite = entryCard.Icon;
			
			MeterPriceText.text = $"{entryCard.MeterPrice}р";
			ComfortText.text = $"{entryCard.Stats.Comfort}/100";
			EcoText.text = $"{entryCard.Stats.Ecology}/100";
			SecurityText.text = $"{entryCard.Stats.Security}/100";
			HousePriceText.text = $"{entryCard.Stats.HousePrice}/100";
			BeautyText.text = $"{entryCard.Stats.Beauty}/100";
			
			Button.onClick.AddListener(() => { disableWindow?.Invoke(); cardAction?.Invoke(); });
		}
		
		public override Type GetCardsType() => typeof(EntryCardSO);
	}
}