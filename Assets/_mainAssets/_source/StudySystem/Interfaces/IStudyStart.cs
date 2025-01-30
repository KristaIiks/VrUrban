using UnityEngine;
using System;

namespace StudySystem
{
	public interface IStudyStart
	{
		[ContextMenu("Start study")]
		void Start(Action OnComplete = null);
	}
}