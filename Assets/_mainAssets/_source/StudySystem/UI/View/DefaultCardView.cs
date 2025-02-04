using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StudySystem
{
	public class DefaultCardView : CardView
	{
		[SerializeField] protected TMP_Text TimeText;
		[SerializeField] protected Button Button;

		public override void Init(Action cardAction, Action disableWindow, CardSO info)
		{
			base.Init(cardAction, disableWindow, info);
			
			if (info is QuestCardSO questInfo)
			{
				TimeText.text = $"Проходить ≈ {questInfo.TimeToComplete} минут";	
			}
			
			Button.onClick.AddListener(() => { cardAction?.Invoke(); disableWindow?.Invoke(); });
		}
	}
}
