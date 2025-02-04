using System;
using UnityEngine;

namespace StudySystem
{
	[CreateAssetMenu(menuName = "Study/Quests/Default", fileName = "New Quest", order = 0)]
	public class DefaultQuest : QuestSO
	{
		public override event Action<QuestResult> OnQuestComplete;
		protected override DateTime Time { get; set; }
		
		private IStudyObject _studyObject;

		public override void StartQuest(GameObject obj)
		{
			base.StartQuest(obj);
			
			_studyObject = obj.GetComponent<IStudyObject>();
			_studyObject.StartDefaultStudy(() => CompleteQuest());
		}
		
		public override void Skip(bool result)
		{
			_studyObject.Skip();
		}
		
		public override void Restart(bool canContinue)
		{
			if(canContinue)
			{
				Time = DateTime.UtcNow;
				_studyObject.Restart(canContinue);
				return;
			}
			
			_studyObject.Restart(canContinue);
		}
		
		public override void CompleteQuest()
		{
		   OnQuestComplete?.Invoke(new QuestResult(
			this,
			true,
			(DateTime.UtcNow - Time).TotalSeconds
		   ));
		}
	}
}