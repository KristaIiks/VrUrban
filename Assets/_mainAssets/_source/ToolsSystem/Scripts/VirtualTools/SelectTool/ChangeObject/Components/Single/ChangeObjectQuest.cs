using System;
using System.Collections.Generic;
using System.Linq;
using SmartConsole;
using StudySystem;
using UnityEngine;

namespace ToolsSystem
{
	[CreateAssetMenu(menuName = "Study/Quests/ChangeObject", fileName = "New Quest", order = 1)]
	public sealed class ChangeObjectQuest : MistakesQuest
	{
		[field:SerializeField] public int[] CorrectObjectsId { get; private set; } = new int[1];
		
		public override event Action<QuestResult> OnQuestComplete;
		
		protected override DateTime Time { get; set; }
		
		private Changeable _changeable;
		private int _currentObjectId = -1;

		//? add support to multiple objects
		public override void StartQuest(GameObject[] objs)
		{
			base.StartQuest(objs);
			
			if (!objs[0].TryGetComponent(out Changeable component))
			{
				SConsole.LogException("ChangeQuest", new MissingComponentException());
				//? Start default quest?
				return;
			}
			
			_changeable = component;
			_changeable.OnObjectChanged += CheckId;
			
			_changeable.CanSelect = true;
		}

		public override void CompleteQuest()
		{
			_changeable.OnObjectChanged -= CheckId;
			_changeable.CanSelect = false;
			
			OnQuestComplete?.Invoke(new QuestResult(
				this,
				CorrectObjectsId.Any((correct) => correct == _currentObjectId),
				(DateTime.UtcNow - Time).TotalSeconds,
				_changeable.Variants[_currentObjectId].Rewards,
				_mistakesCount
		   ));
		}

		public override void Skip(bool result)
		{
			_changeable.Restart(false);
			
			if (result)
			{
				_changeable.ChangeBuild(CorrectObjectsId[UnityEngine.Random.Range(0, CorrectObjectsId.Length)]);
			}
			else
			{
				List<int> mistakesId = new List<int>();
				for (int i = 0; i < _changeable.Variants.Count; i++)
				{
					if (!CorrectObjectsId.Contains(i))
					{
						mistakesId.Add(i);
					}
				}
				
				if (mistakesId.Count == 0)
				{
					SConsole.LogException("Skipping", new IndexOutOfRangeException());
					return;
				}
				
				_changeable.ChangeBuild(mistakesId[UnityEngine.Random.Range(0, mistakesId.Count)]);
			}
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
			
			if (CorrectObjectsId.All((correct) => correct != id))
			{
				_mistakesCount++;
				
				if (RemoveOnMistake)
					RemoveMistake();
			}
			
			CompleteQuest();
		}

		protected override void RemoveMistake() => _changeable.HideVariant(_currentObjectId);
	}
}