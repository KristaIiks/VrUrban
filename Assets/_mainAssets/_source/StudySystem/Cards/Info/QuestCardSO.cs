using UnityEngine;

namespace StudySystem
{
	[CreateAssetMenu(menuName = "Study/Card/Default", fileName = "Card Info", order = 0)]
	public class QuestCardSO : CardSO
	{
		[field:Space(10)]
		[field:SerializeField] public Sprite Icon { get; private set; }
		[field:SerializeField] public int TimeToComplete { get; private set; }
	}
}