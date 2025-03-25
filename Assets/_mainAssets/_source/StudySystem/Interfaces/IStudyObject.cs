using System;

namespace StudySystem
{
	//? refactor
	public interface IStudyObject: IStudyEvents
	{
		void StartDefaultStudy(Action OnComplete);
		void Skip();
		void Restart(bool canContinue = true);
	}
}