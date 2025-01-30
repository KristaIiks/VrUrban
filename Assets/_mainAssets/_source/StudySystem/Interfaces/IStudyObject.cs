using UnityEngine;

namespace StudySystem
{
	public interface IStudyObject: IStudyStart, IStudyComplete
	{	
		[ContextMenu("Restart")]
		void Restart(bool canContinue = true);
	}
}