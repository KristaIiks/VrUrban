namespace StudySystem
{
	public struct QuestResult
	{
		public QuestSO Quest;
		public bool IsCorrect;
		public uint WrongAnswers;
		public double Time;
		public RewardStats? Reward;
		
		public QuestResult(QuestSO quest, bool correct, double time, RewardStats? reward = null, uint errors = 0)
		{
			Quest = quest;
			IsCorrect = correct;
			Time = time;
			Reward = reward;
			WrongAnswers = errors;
		}
	}
}