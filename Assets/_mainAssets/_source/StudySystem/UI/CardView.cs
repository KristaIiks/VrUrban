using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StudySystem
{
	public class CardView : MonoBehaviour
	{
		[SerializeField] private Image Image;
		[SerializeField] private TMP_Text NameText;
		[SerializeField] private TMP_Text DescriptionText;
		[SerializeField] private TMP_Text TimeText;
		[SerializeField] private Button Button;
		
		public void Init(Action cardAction, Action disableWindow, CardSO info)
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
