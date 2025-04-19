using System;
using System.Collections.Generic;
using System.Linq;
using SmartConsole;
using StudySystem;
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
		[Space(25)]
		[SerializeField] private UnityEvent OnBranchComplete;
		
		[HideInInspector] public Card _currentCard; // TODO: private
		private bool _isStarted;
		
		public readonly BranchResult result;
		
		// TODO: add onAwake param
		public void Start() => StartBranch();
		
		[ContextMenu("Start study")]
		public void StartBranch()
		{
			if(_isStarted) //? restarting
				return;
			
			#if UNITY_EDITOR
			
			if (DebugSkip)
			{
				List<Card> listToSkip = FindPath(new List<Card>(){FirstCard}, FirstCard, DebugSkip);
				
				if(listToSkip == null)
				{
					SConsole.Log(LOG_TAG, "Cant find path to card!", LogType.Warning);
				}
				else
				{
					// TODO: disable visuals (other skip in cards)
					listToSkip[0].StartCard(() => OnBranchComplete?.Invoke(), this);
					CardsWindow.Instance.HideCards(); // temp solution
					
					for (int i = 0; i < listToSkip.Count - 1; i++)
					{
						listToSkip[i].Skip(listToSkip[i + 1]);
					}
					
					SConsole.Log(LOG_TAG, "Skip successful!", 2);
				}
			}
			else
			{
				if (FirstCard.Info != null)
				{
					CardsWindow.Instance.DisplayCards(
						() => OnBranchComplete?.Invoke(), 
						this, 
						new Card[] { FirstCard }
					);
				}
				else
				{
					FirstCard.StartCard(() => OnBranchComplete?.Invoke(), this);
				}
			}
			
			#else
			if (FirstCard.Info != null)
			{
				CardsWindow.Instance.DisplayCards(
					() => OnBranchComplete?.Invoke(), 
					this, 
					new Card[] { FirstCard }
				);
			}
			else
			{
				FirstCard.StartCard(() => OnBranchComplete?.Invoke(), this);
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
		
		public void CompleteCard(List<QuestResult> results, RewardStats? cardStats = null)
		{
			foreach (QuestResult result in results)
			{
				this.result.Mistakes += result.WrongAnswers;
				this.result.Time += result.Time;
				
				if (result.Reward != null)
					ChangeStats((RewardStats)result.Reward);
			}
			
			if (cardStats != null)
				ChangeStats((RewardStats)cardStats);
		}
		
		private void ChangeStats(RewardStats stats)
		{			
			result.Stats.HousePrice += stats.HousePrice;
			result.Stats.Beauty += stats.Beauty;
			result.Stats.Comfort += stats.Comfort;
			result.Stats.Ecology += stats.Ecology;
			result.Stats.Security += stats.Security;
		}
		
		private List<Card> FindPath(List<Card> path, Card card, Card find)
		{	
			if (card.GetCards().Contains(find)) { path.Add(find); return path; }
			
			foreach (var item in card.GetCards())
			{
				List<Card> tmp = new List<Card>(path) {item};
				tmp = FindPath(tmp, item, find);
				
				if (tmp != null)
					return tmp;
			}
			
			return null;
		}
	}
}

// TODO: move to other file
//? other system (own by card res)
public class BranchResult
{
	public uint Mistakes;
	public double Time;
	public int Money;
	
	public RewardStats Stats;
}