using UnityEngine;

namespace StudySystem
{
	[CreateAssetMenu(menuName = "Study", fileName = "Card Info", order = 0)]
	public class EntryCardSO : CardSO
	{
		[field:SerializeField] public EntryCardStats Stats { get; private set; }
	}
	
	[System.Serializable]
	public struct EntryCardStats
	{
		public int MeterPrice;
		[Range(0, 100)] public int Comfort;
		[Range(0, 100)] public int Ecology;
		[Range(0, 100)] public int Security;
		[Range(0, 100)] public int Price;
		[Range(0, 100)] public int Beauty;
	}
}