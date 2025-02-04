using System;
using System.Collections.Generic;
using SmartConsole;
using UnityEngine;
using UnityEngine.Events;

namespace StudySystem
{
	public abstract class Card : MonoBehaviour
	{
		protected static string LOG_TAG = "Study";
		
		[field:SerializeField] public CardSO Info { get; private set; }
		
		[Header("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"), Space(25)]
		[SerializeField] protected UnityEvent OnStart;
		[SerializeField] protected UnityEvent OnComplete;
		
		public bool IsCompleted { get; protected set; }
				
		protected abstract List<Card> _allCards { get; set; }
		
		protected Action<Card> updateBranch;
		protected Action m_previousCard;
		
		public virtual void StartCard(Action previousCard, Action<Card> updateBranch)
		{
			m_previousCard = previousCard;
			this.updateBranch = updateBranch;
			updateBranch.Invoke(this);
			
			OnStart?.Invoke();
		}
		
		protected virtual void Continue()
		{
			// if has only 1 card force start next without drawing
			if (_allCards.Count == 1 || !_allCards[0].IsCompleted)
			{
				_allCards[0].StartCard(() => Continue(), updateBranch);
				return;
			}
		}
		
		public abstract void SkipAll();
		public virtual void Skip(Card to)
		{
			if(!_allCards.Contains(to))
			{ 
				SConsole.Log(
					LOG_TAG, 
					"Card not found in list of this card. Error path finding.", 
					LogType.Error, 
					gameObject
				); 
				return; 
			}
			
			Restart(false); // reset player changes and deactivate quests
			IsCompleted = true;
			
			to.StartCard(() => Continue(), updateBranch);
		}
		
		public void Restart(RestartType type)
		{
			switch (type)
			{
				case RestartType.Current:
					Restart();
					break;
				case RestartType.All:
					foreach (Card card in _allCards)
					{
						card.Restart(type);
					}
					
					Restart(false);
				break;
			}
		}
		
		protected virtual void Restart(bool canContinue = true)
		{
			IsCompleted = false;
		}
		
		public IReadOnlyList<Card> GetCards() => _allCards;
	}
}