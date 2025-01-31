using System;
using System.Collections.Generic;
using System.Linq;
using SmartConsole;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace StudySystem
{
	[RequireComponent(typeof(AudioSource))]
	public abstract class Card : MonoBehaviour
	{
		private static string LOG_TAG = "Study";
		
		[SerializeField] protected PlayableDirector Cutscene;
		[SerializeField] public CardSO CardInfo { get; private set; }
		[SerializeField] protected UnityEvent OnStart;
		[SerializeField] protected List<QuestStatus> Quests;
		[SerializeField] protected UnityEvent OnComplete;
		[field:SerializeField] public bool IsCompleted { get; protected set; }
		[Interface(typeof(IStudyInit), typeof(IStudyComplete)), SerializeField] private GameObject[] InitObjects;
				
		protected List<Card> _allCards;
		protected AudioSource _audio; // TODO: move to bot audio
		
		protected Action<Card> updateBranch;
		protected Action m_previousCard;
		
		protected virtual void OnValidate()
		{
			_audio ??= GetComponent<AudioSource>();
		}
		
		protected virtual void Awake()
		{
			foreach (QuestStatus quest in Quests)
			{
				if(!quest.Object.TryGetComponent(out IStudyObject component))
				{
					SConsole.Log(LOG_TAG, "Quest is null", LogType.Warning);
					continue;
				}
				
				OnStart.AddListener(() => component.InitStudy());
				OnComplete.AddListener(() => component.OnStudyComplete());
			}
			
			foreach (GameObject obj in InitObjects)
			{
				if (obj == null) { SConsole.Log(LOG_TAG, "Can't init null object", LogType.Warning); continue; }
				
				if (obj.TryGetComponent(out IStudyInit start))
				{
					OnStart.AddListener(() => start.InitStudy());
				}
				
				if (obj.TryGetComponent(out IStudyComplete complete))
				{
					OnComplete.AddListener(() => complete.OnStudyComplete());
				}
			}
		}
		
		public virtual void StartCard(Action previousCard, Action<Card> updateBranch)
		{
			m_previousCard = previousCard;
			this.updateBranch = updateBranch;
			updateBranch.Invoke(this);
			
			OnStart?.Invoke();
			
			foreach (QuestStatus status in Quests)
			{
				status.StartQuest();
				status.OnComplete.AddListener(QuestCompleted);
			}
			
			Cutscene.Play();
		}
		
		protected virtual void QuestCompleted(QuestResult result)
		{			
			if (Quests.All((quest) => quest.IsCompleted))
			{
				// TODO: card reward
				Continue();
			}
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
			Quests.ForEach((quest) => quest.Restart(canContinue));
			IsCompleted = false;
		}
		
		public IReadOnlyList<Card> GetCards() => _allCards;
	}
}