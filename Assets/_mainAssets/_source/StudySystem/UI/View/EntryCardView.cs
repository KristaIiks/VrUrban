using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StudySystem
{
	public sealed class EntryCardView : CardView
	{
		[SerializeField] private TMP_Text MeterPriceText;
		[SerializeField] private TMP_Text ComfortText;
		[SerializeField] private TMP_Text EcoText;
		[SerializeField] private TMP_Text SecurityText;
		[SerializeField] private TMP_Text PriceText;
		[SerializeField] private TMP_Text BeautyText;
		[SerializeField] private Button Button;
		
		public override void Init(Action cardAction, Action disableWindow, CardSO info)
		{
			base.Init(cardAction, disableWindow, info);
			
			if (info is EntryCardSO entryCardInfo)
			{
				MeterPriceText.text = $"{entryCardInfo.Stats.MeterPrice}р";
				ComfortText.text = $"{entryCardInfo.Stats.Comfort}/100";
				EcoText.text = $"{entryCardInfo.Stats.Ecology}/100";
				SecurityText.text = $"{entryCardInfo.Stats.Security}/100";
				PriceText.text = $"{entryCardInfo.Stats.Price}/100";
				BeautyText.text = $"{entryCardInfo.Stats.Beauty}/100";
			}
			
			Button.onClick.AddListener(() => { cardAction?.Invoke(); disableWindow?.Invoke(); });
		}
	}
}