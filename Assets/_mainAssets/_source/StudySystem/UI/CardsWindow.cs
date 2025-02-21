using System;
using SmartConsole;
using UnityEngine;

namespace StudySystem
{
	public class CardsWindow : MonoBehaviour
	{
		public static CardsWindow Instance;
		
		[SerializeField] private GameObject Window;
		[SerializeField] private CardView[] Views;
		
		[SerializeField] private Transform Content;
		
		private void Awake()
		{
			Instance = this;
			HideCards();
		}
		
		public void DisplayCards(Action previousCard, Branch branch, Card[] cards)
		{
			ClearViewCards();
			
			foreach (Card card in cards)
			{
				if (card.IsCompleted) { continue; }
				if (!card.Info) { SConsole.LogException("Display", new NullReferenceException(), card); }
				
				foreach (CardView view in Views)
				{
					if (view.GetCardsType() == card.Info.GetType())
					{
						CardView template = Instantiate(view, Content, false);
						
						template.Init(
							() => card.StartCard(previousCard, branch), 
							() => HideCards(), 
							card.Info
						);
						
						break;
					}
				}
			}
			
			Window.SetActive(true);
		}
		
		public void HideCards() => Window.SetActive(false);
		
		private void ClearViewCards()
		{
			foreach (Transform item in Content)
			{
				Destroy(item.gameObject);
			}
		}
	}
}
