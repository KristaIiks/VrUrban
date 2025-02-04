using System;
using System.Collections.Generic;
using System.Linq;
using SmartConsole;
using UnityEngine;
using UnityEngine.Events;

namespace StudySystem
{
	public sealed class Branch : MonoBehaviour
	{
		private const string LOG_TAG = "Study";		
		[SerializeField] private Card FirstCard;
#if UNITY_EDITOR
		[SerializeField] private Card DebugSkip;
		[Space(25)]
#endif
		[SerializeField] private UnityEvent OnBranchComplete;
		
		private Card _currentCard;
		private bool _isStarted;
		
		[ContextMenu("Start study")]
		public void StartBranch()
		{
			if(_isStarted) { return; }
			
			#if UNITY_EDITOR
			if(DebugSkip)
			{
				List<Card> listToSkip = FindPath(new List<Card>(){FirstCard}, FirstCard, DebugSkip);
				if(listToSkip == null)
				{
					SConsole.Log(LOG_TAG, "Cant find path to card!", LogType.Warning);
				}
				else
				{
					listToSkip[0].StartCard(() => OnBranchComplete?.Invoke(), (card) => _currentCard = card);
					
					for (int i = 0; i < listToSkip.Count - 1; i++)
					{
						listToSkip[i].Skip(listToSkip[i + 1]);
					}
					SConsole.Log(LOG_TAG, "Skip successful!", 2);
				}
			}
			#else
			if (FirstCard.Info != null)
			{
				CardsWindow.Instance.DisplayCards(
					() => OnBranchComplete?.Invoke(), 
					(card) => _currentCard = card, 
					new Card[] { FirstCard }
				);
			}
			else
			{
				FirstCard.StartCard(() => OnBranchComplete?.Invoke(), (card) => _currentCard = card);
			}
			#endif
			
			SConsole.Log(LOG_TAG, $"Start study branch #{gameObject.name}", 2, gameObject);
		}
		
		[ContextMenu("Restart current")]
		// TODO: add to menu or btn
		private void RestartCurrent()
		{
			SConsole.Log(LOG_TAG, $"Restart card - {_currentCard.name}", 2, _currentCard.gameObject);
			_currentCard.Restart(RestartType.Current);
		}
		
		[ContextMenu("Restart full")]
		private void RestartBranch()
		{
			FirstCard.Restart(RestartType.All);
			SConsole.Log(LOG_TAG, $"Restarting branch #{gameObject.name}...", 2, gameObject);
			
			_isStarted = false;
			StartBranch();
		}
		
		private List<Card> FindPath(List<Card> path, Card card, Card find)
		{	
			if(card.GetCards().Contains(find)) { path.Add(find); return path; }
			
			foreach (var item in card.GetCards())
			{
				List<Card> tmp = new List<Card>(path) {item};
				tmp = FindPath(tmp, item, find);
				
				if(tmp != null)
				{
					return tmp;
				}
			}
			return null;
		}
	}
}