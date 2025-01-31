using System;
using UnityEngine;

namespace StudySystem
{
	public interface IStudyObject: IStudyEvents
	{
		void IStudyInit() => Restart(true);
		void StartDefaultStudy(Action OnComplete);
		
		[ContextMenu("Restart")]
		void Restart(bool canContinue = true);
	}
}