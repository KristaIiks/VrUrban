using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StudySystem
{
	public class CardView : MonoBehaviour
	{
		[SerializeField] protected Image Image;
		[SerializeField] protected TMP_Text NameText;
		[SerializeField] protected TMP_Text DescriptionText;
		[SerializeField] protected TMP_Text TimeText;
		[SerializeField] protected Button Button;
		
		public virtual void Init(Action cardAction, Action disableWindow, CardSO info)
		{
			Image.sprite = info.Icon;
			NameText.text = info.Name;
			DescriptionText.text = info.Description;
			TimeText.text = $"Проходить ≈ {info.TimeToComplete} минут";	
			
			Button.onClick.AddListener(() => cardAction?.Invoke());
			Button.onClick.AddListener(() => disableWindow?.Invoke());
		}
	}
}
