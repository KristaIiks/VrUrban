using UnityEngine;

namespace StudySystem
{
	public abstract class CardSO : ScriptableObject
	{
		[field:SerializeField, TextArea(1, 2)] public string Name { get; private set; } = "None";
		[field:SerializeField, TextArea(2, 6)] public string Description { get; private set; } = "Sample description";
	}
}