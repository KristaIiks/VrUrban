using UnityEngine;

namespace StudySystem
{
	[CreateAssetMenu(menuName = "Study/Card/Entry", fileName = "Card Info", order = 1)]
	public class EntryCardSO : CardSO
	{
		[field:Space(25)]
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