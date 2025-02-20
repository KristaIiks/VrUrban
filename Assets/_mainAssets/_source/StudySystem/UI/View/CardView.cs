using System;
using UnityEngine;

namespace StudySystem
{
	public abstract class CardView: MonoBehaviour
	{
		public abstract void Init(Action onClick, Action disableWindow, CardSO info);
		public abstract Type GetCardsType();
	}
}
