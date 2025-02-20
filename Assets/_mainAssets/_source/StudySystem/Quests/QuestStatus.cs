using UnityEngine;
using UnityEngine.Events;

namespace StudySystem
{
	[System.Serializable]
	public class QuestStatus
	{
		[field:SerializeField, Interface(typeof(IStudyObject))] public GameObject[] Objects { get; private set; }
		[SerializeField] private QuestSO Info;
		
		[HideInInspector] public UnityEvent<QuestResult> OnComplete;
		
		public QuestResult Result { get; private set; }
		
		public bool IsCompleted
		{ 
			get => m_isCompleted; 
			private set { m_isCompleted = value; _inProgress = !value; } 
		}
		
		private bool m_isCompleted;
		private bool _inProgress;
		
		public void StartQuest()
		{
			IsCompleted = false;
			Info.StartQuest(Objects);
			
			Info.OnQuestComplete += CompleteQuest;
		}
		
		public void Skip(bool result = true)
		{
			OnComplete.RemoveAllListeners();
			
			Info.StartQuest(Objects);
			Info.Skip(result);
			
			IsCompleted = true;
		}
		
		public void Restart(bool canContinue)
		{
			if(!canContinue)
			{
				IsCompleted = false;
				_inProgress = false;
				
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