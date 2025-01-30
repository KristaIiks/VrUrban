namespace StudySystem
{
	public struct QuestResult
	{
		public QuestSO Quest;
		public bool IsCorrect;
		public int WrongAnswers;
		public double Time;
		
		public QuestResult(QuestSO quest, bool correct, double time, int errors = 0)
		{
			Quest = quest;
			IsCorrect = correct;
			Time = time;
			WrongAnswers = errors;
		}
	}
}
