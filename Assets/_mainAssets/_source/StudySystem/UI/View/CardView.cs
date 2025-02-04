using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StudySystem
{
	public abstract class CardView : MonoBehaviour
	{
		[SerializeField] protected TMP_Text NameText;
		[SerializeField] protected TMP_Text DescriptionText;
		[SerializeField] protected Image Image;
		
		public virtual void Init(Action cardAction, Action disableWindow, CardSO info)
		{
			Image.sprite = info.Icon;
			NameText.text = info.Name;
			DescriptionText.text = info.Description;
		}
	}
}
