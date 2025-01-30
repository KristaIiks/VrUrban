namespace StudySystem
{
	[System.Serializable]
	public struct CardReward
	{
		public int Comfort { get; private set; }
		public int Ecology { get; private set; }
		public int Criminal { get; private set; }
		public int HouseCost { get; private set; }
	}
}
