using System;
using System.Collections.Generic;
using System.Linq;
using SmartConsole;
using UnityEngine;

namespace StudySystem
{
	public abstract class QuestCard : Card
	{
		[Space(25), Header("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"), Space(25)]
		
		[SerializeField] protected List<QuestStatus> Quests = new List<QuestStatus>();
		[Interface(typeof(IStudyInit), typeof(IStudyComplete)), SerializeField] private GameObject[] InitObjects;
		
		protected virtual void Awake()
		{
			foreach (QuestStatus quest in Quests)
			{
				if (quest.Objects.Length == 0)
				{
					SConsole.Log(LOG_TAG, "Quest objects are empty", LogType.Warning, gameObject);
					continue;
				}
				
				foreach (GameObject obj in quest.Objects)
				{
					if(obj.TryGetComponent(out IStudyObject component))
					{
						OnStart.AddListener(() => component.InitStudy());
						OnComplete.AddListener(() => component.OnStudyComplete());
					}
					else
					{
						SConsole.Log(LOG_TAG, "Quest object is null", LogType.Warning, gameObject);
					}
				}
			}
			
			foreach (GameObject obj in InitObjects)
			{
				if (obj == null) { SConsole.Log(LOG_TAG, "Can't init null object", LogType.Warning, gameObject); continue; }
				
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

		public override void StartCard(Action previousCard, Branch branch)
		{
			base.StartCard(previousCard, branch);
			
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
			to.StartCard(() => Continue(), m_branch);
		}

		protected virtual void QuestCompleted(QuestResult result)
		{			
			if (Quests.All((quest) => quest.IsCompleted))
			{
				List<QuestResult> res = new List<QuestResult>();
				res.AddRange(Quests.Select((quest) => quest.Result));
				
				m_branch.CompleteCard(res);
				
				OnComplete?.Invoke();
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