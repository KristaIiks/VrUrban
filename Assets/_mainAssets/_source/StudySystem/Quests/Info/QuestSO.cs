using System;
using UnityEngine;

namespace StudySystem
{
	public abstract class QuestSO : ScriptableObject
	{		
		public abstract event Action<QuestResult> OnQuestComplete;
		protected abstract DateTime Time { get; set; }
		
		public virtual void StartQuest(GameObject[] objs)
		{
			Time = DateTime.UtcNow;
		}
		public abstract void Skip(bool result);
		public abstract void Restart(bool canContinue);
		public abstract void CompleteQuest();
	}
}