using UnityEngine;

namespace StudySystem
{
	[CreateAssetMenu(menuName = "Study", fileName = "Card Info", order = 0)]
	public class CardSO : ScriptableObject
	{
		[field:SerializeField] public string Name { get; private set; } = "None";
		[field:SerializeField, Multiline] public string Description { get; private set; } = "Sample description";
		[field:SerializeField] public Sprite Icon { get; private set; }
		[field:SerializeField] public CardView View { get; private set; }
		[field:SerializeField] public int TimeToComplete { get; private set; }
		[field:SerializeField] public CardReward Rewards { get; private set; }
	}
}