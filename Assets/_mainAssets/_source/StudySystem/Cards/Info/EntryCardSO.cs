using UnityEngine;

namespace StudySystem
{
	[CreateAssetMenu(menuName = "Study/Card/Entry", fileName = "Card Info", order = 1)]
	public class EntryCardSO : CardSO
	{
		[field:Space(25)]
		[field:SerializeField] public Sprite Icon { get; private set; }
		[field:SerializeField] public int MeterPrice { get; private set; }
		[field:SerializeField] public RewardStats Stats { get; private set; }
	}
	
	[System.Serializable]
	public struct RewardStats
	{
		public int Comfort;
		public int Ecology;
		public int Security;
		public int HousePrice;
		public int Beauty;
		
		[Space(25)]
		public int Money;
	}
}