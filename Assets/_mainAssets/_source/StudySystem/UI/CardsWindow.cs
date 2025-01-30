using System;
using UnityEngine;

namespace StudySystem
{
	public class CardsWindow : MonoBehaviour
	{
		[SerializeField] private GameObject Window;
		[SerializeField] private CardView CardPrefab;
		[SerializeField] private Transform Content;
		
		public void DisplayCards(Action previousCard, Action<Card> updateBranch, Card[] cards)
		{
			ClearViewCards();
			
			foreach (Card card in cards)
			{
				if (card.IsCompleted) { continue; }
				
				CardView view = Instantiate(CardPrefab, Content, false);
				view.Init(() => card.StartCard(previousCard, updateBranch), () => HideCards(), card.CardInfo);
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
