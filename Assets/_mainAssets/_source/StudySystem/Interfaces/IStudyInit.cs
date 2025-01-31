using UnityEngine;
using System;

namespace StudySystem
{
	public interface IStudyInit
	{
		[ContextMenu("Start study")]
		void InitStudy() { }
	}
}