using System;
using SmartConsole;
using StudySystem;
using UnityEngine;

namespace ToolsSystem
{
	[CreateAssetMenu(menuName = "Study/Quests/ChangeObject/Single", fileName = "New Quest", order = 0)]
	public class ChangeObjectQuest : QuestSO
	{
		[field:SerializeField] public int ObjectId { get; private set; }
		[field:SerializeField] public bool CanMistake { get; private set; }
		
		protected override DateTime Time { get; set; }

		public override event Action<QuestResult> OnQuestComplete;
		
		private Changeable _changeable;
		private int _currentObjectId = -1;
		private int _mistakesCount;

		public override void StartQuest(GameObject obj)
		{
			base.StartQuest(obj);
			
			if (!obj.TryGetComponent(out Changeable component))
			{
				SConsole.LogException("ChangeQuest", new MissingComponentException());
				return;
			}
			
			_changeable = component;
			_changeable.OnObjectChanged += CheckId;
		}

		public override void CompleteQuest()
		{
			_changeable.OnObjectChanged -= CheckId;
			
			OnQuestComplete?.Invoke(new QuestResult(
			this,
			_currentObjectId == ObjectId,
			(DateTime.UtcNow - Time).TotalSeconds,
			_mistakesCount
		   ));
		}

		public override void Restart(bool canContinue)
		{
			if (!canContinue && _changeable != null) { _changeable.OnObjectChanged -= CheckId; }
			
			_mistakesCount = 0;
			
			if (canContinue)
			{
				Time = DateTime.UtcNow;
			}
			
			_changeable.Restart(canContinue);
		}
		
		private void CheckId(int id)
		{
			_currentObjectId = id;
			
			if (id != ObjectId && CanMistake)
			{
				_mistakesCount++;
			}
			
			if (CanMistake || id == ObjectId) 
			{
				CompleteQuest();
			}
		}
	}
}
