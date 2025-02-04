using System;
using UnityEngine;
using UnityEngine.Events;

namespace StudySystem
{
	[Serializable]
	public struct QuestStatus
	{
		[field:SerializeField, Interface(typeof(IStudyObject))] public GameObject Object { get; private set; }
		[SerializeField] private QuestSO Info;
		
		[HideInInspector] public UnityEvent<QuestResult> OnComplete;
		
		public QuestResult Result { get; private set; }
		public bool IsCompleted { get; private set; }
		
		public void StartQuest()
		{
			Info.StartQuest(Object);
			Info.OnQuestComplete += CompleteQuest;
		}
		
		public void Skip(bool result = true)
		{
			OnComplete.RemoveAllListeners();
			Info.Skip(result);			
		}
		
		public void Restart(bool canContinue)
		{
			if(!canContinue)
			{
				IsCompleted = false;
				
				OnComplete.RemoveAllListeners();
				Info.OnQuestComplete -= CompleteQuest;
			}
			
			Info.Restart(canContinue);
		}
		
		private void CompleteQuest(QuestResult result)
		{
			Info.OnQuestComplete -= CompleteQuest;
			
			IsCompleted = true;
			Result = result;
			
			OnComplete?.Invoke(result);
			OnComplete.RemoveAllListeners();
		}
	}
}