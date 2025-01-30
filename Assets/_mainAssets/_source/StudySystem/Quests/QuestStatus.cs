using System;
using UnityEngine;

namespace StudySystem
{
	[Serializable]
	public struct QuestStatus
	{
		[field:SerializeField, Interface(typeof(IStudyObject))] public GameObject Object { get; private set; }
		[SerializeField] private QuestSO Info;
		
		public event Action<QuestResult> OnComplete;
		public bool IsCompleted { get; private set; }
		private QuestResult _result;
		
		public void StartQuest()
		{
			Info.StartQuest(Object);
			Info.OnQuestComplete += CompleteQuest;
		}
		
		public void Restart(bool canContinue)
		{
			Info.Restart(canContinue);
			
			if(!canContinue)
			{
				IsCompleted = false;
			}
		}
		
		public void CompleteQuest(QuestResult result)
		{
			Info.OnQuestComplete -= CompleteQuest;
			
			IsCompleted = true;
			_result = result;
			
			OnComplete?.Invoke(result);
		}

		public QuestResult GetResult() => _result;
	}
}