using UnityEngine;
using System;

namespace StudySystem
{
	/// <summary>
	/// Contains quest info and complete logic
	/// </summary>
	public abstract class QuestSO : ScriptableObject
	{
		[field:SerializeField] public string Name { get; private set; } = "None";
		[field:SerializeField, Multiline] public string Description { get; private set; } = "Sample description";
		[field:SerializeField] public Sprite Icon { get; private set; }
		
		public abstract event Action<QuestResult> OnQuestComplete;
		protected abstract DateTime Time { get; set; }
		
		public virtual void StartQuest(GameObject obj) { Time = DateTime.UtcNow; }
		public abstract void Restart(bool canContinue);
		public abstract void CompleteQuest();
	}
}
