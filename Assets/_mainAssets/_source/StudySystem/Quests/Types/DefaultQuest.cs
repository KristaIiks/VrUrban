using System;
using System.Collections.Generic;
using UnityEngine;

namespace StudySystem
{
	[CreateAssetMenu(menuName = "Study/Quests/Default", fileName = "New Quest", order = 0)]
	public class DefaultQuest : QuestSO
	{
		public override event Action<QuestResult> OnQuestComplete;
		protected override DateTime Time { get; set; }
		
		private List<IStudyObject> _studyObjects = new List<IStudyObject>();
		private int _questsCompleted;

		public override void StartQuest(GameObject[] objs)
		{
			base.StartQuest(objs);
			
			foreach (GameObject obj in objs)
			{
				if (obj.TryGetComponent(out IStudyObject component))
				{
					_studyObjects.Add(component);
					component.StartDefaultStudy(() => CompleteQuest());
				}
			}
		}
		
		public override void Skip(bool result)
		{
			_studyObjects.ForEach((obj) => obj.Skip());
		}
		
		public override void Restart(bool canContinue)
		{
			if(canContinue)
			{
				Time = DateTime.UtcNow;
			}
			
			_studyObjects.ForEach((obj) => obj.Restart(canContinue));
		}
		
		public override void CompleteQuest()
		{
			_questsCompleted++;
			if (_questsCompleted != _studyObjects.Count) { return; }
			
			OnQuestComplete?.Invoke(new QuestResult(
				this,
				true,
				(DateTime.UtcNow - Time).TotalSeconds
			));
		}
	}
}