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
				if(listToSkip.Count == 0)
				{
					SConsole.Log(LOG_TAG, "Cant find path to card!", LogType.Warning);
				}
				else
				{
					listToSkip.Add(DebugSkip);
					SConsole.Log(LOG_TAG, listToSkip.Count, 2);
					for (int i = 0; i < listToSkip.Count - 1; i++)
					{
						SConsole.Log(LOG_TAG, listToSkip[i].name, 2);
						listToSkip[i].Skip(listToSkip[i + 1]);
					}
					FirstCard = DebugSkip;
					SConsole.Log(LOG_TAG, "Skip successful!", 2);
				}
			}
			#endif
			FirstCard.StartCard(() => OnBranchComplete?.Invoke(), (card) => _currentCard = card);
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
			if(card.GetCards().Contains(find)) { path.Add(card); return path; }
			
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