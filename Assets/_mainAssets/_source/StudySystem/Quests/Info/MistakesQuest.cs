using UnityEngine;

namespace StudySystem
{
	public abstract class MistakesQuest : QuestSO
	{
		[field:SerializeField] public bool RemoveOnMistake { get; protected set; } = true;
		protected uint _mistakesCount;
		
		protected abstract void RemoveMistake(); 
	}
}