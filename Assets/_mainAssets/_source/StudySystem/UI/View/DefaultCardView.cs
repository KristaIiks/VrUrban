using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StudySystem
{
	public class DefaultCardView : CardView
	{
		[SerializeField] protected TMP_Text NameText;
		[SerializeField] protected TMP_Text DescriptionText;
		[SerializeField] protected Image Image;
		[SerializeField] protected TMP_Text TimeText;
		[SerializeField] protected Button Button;


		public override void Init(Action cardAction, Action disableWindow, CardSO info)
		{
			if (info is not QuestCardSO questCard) { return; }
			
			NameText.text = questCard.Name;
			DescriptionText.text = questCard.Description;
			Image.sprite = questCard.Icon;
			
			Button.onClick.AddListener(() => { cardAction?.Invoke(); disableWindow?.Invoke(); });
		}
		
		public override Type GetCardsType() => typeof(QuestCardSO);
	}
}
