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
		public bool IsCompleted { get; private set; }
		
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