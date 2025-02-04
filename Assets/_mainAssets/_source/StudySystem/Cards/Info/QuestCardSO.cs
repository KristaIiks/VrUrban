using UnityEngine;

namespace StudySystem
{
	[CreateAssetMenu(menuName = "Study/Card/Default", fileName = "Card Info", order = 0)]
	public class QuestCardSO : CardSO
	{
		[field:SerializeField] public int TimeToComplete { get; private set; }
	}
}