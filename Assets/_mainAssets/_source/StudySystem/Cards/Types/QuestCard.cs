using System;
using System.Collections.Generic;
using System.Linq;
using SmartConsole;
using UnityEngine;
using UnityEngine.Playables;

namespace StudySystem
{
	public abstract class QuestCard : Card
	{
		[Space(25), Header("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"), Space(25)]
		[SerializeField] protected PlayableDirector Cutscene;
		
		[Space(10)]
		[SerializeField] protected List<QuestStatus> Quests;
		[Interface(typeof(IStudyInit), typeof(IStudyComplete)), SerializeField] private GameObject[] InitObjects;
		
		protected virtual void Awake()
		{
			foreach (QuestStatus quest in Quests)
			{
				if (quest.Object == null)
				{
					SConsole.Log(LOG_TAG, "Quest is null", LogType.Warning);
					continue;
				}
				
				IStudyObject component = quest.Object.GetComponent<IStudyObject>();
				
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

		public override void StartCard(Action previousCard, Action<Card> updateBranch)
		{
			base.StartCard(previousCard, updateBranch);
			
			// TODO: Cutscene?.Play();
			
			foreach (QuestStatus status in Quests)
			{
				status.StartQuest();
				status.OnComplete.AddListener(QuestCompleted);
			}
		}

		public override void Skip(Card to)
		{
			base.Skip(to);
			
			Quests.ForEach((quest) => quest.Skip());
		}

		protected virtual void QuestCompleted(QuestResult result)
		{			
			if (Quests.All((quest) => quest.IsCompleted))
			{
				// TODO: card reward
				Continue();
			}
		}
		
		protected override void Restart(bool canContinue = true)
		{
			base.Restart(canContinue);
			Quests.ForEach((quest) => quest.Restart(canContinue));
		}
	}
}