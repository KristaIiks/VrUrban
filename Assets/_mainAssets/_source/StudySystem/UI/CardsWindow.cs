using System;
using UnityEngine;

namespace StudySystem
{
	public class CardsWindow : MonoBehaviour
	{
		public static CardsWindow Instance;
		
		[SerializeField] private GameObject Window;
		
		[SerializeField] private CardView DefaultView;
		[SerializeField] private EntryCardView EntryCardView;
		
		[SerializeField] private Transform Content;
		
		private void Awake()
		{
			Instance = this;
		}
		
		public void DisplayCards(Action previousCard, Action<Card> updateBranch, Card[] cards)
		{
			Debug.LogWarning("Display new");
			// ClearViewCards();
			
			// foreach (Card card in cards)
			// {
			// 	if (card.IsCompleted) { continue; }
				
			// 	CardView type = card.Info is EntryCardSO ? EntryCardView : DefaultView;
				
			// 	CardView view = Instantiate(type, Content, false);
			// 	view.Init(() => card.StartCard(previousCard, updateBranch), () => HideCards(), card.Info);
			// }
			
			// Window.SetActive(true);
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
