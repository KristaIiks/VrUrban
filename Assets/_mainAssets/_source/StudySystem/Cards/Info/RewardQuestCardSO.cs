using UnityEngine;

namespace StudySystem
{
	[CreateAssetMenu(menuName = "Study/Card/Reward", fileName = "Card Info", order = 2)]
	public class RewardQuestCardSO : QuestCardSO
	{
		[field:SerializeField] public int Price { get; private set; }
		[field:SerializeField] public RewardStats Stats { get; private set; }
	}
}