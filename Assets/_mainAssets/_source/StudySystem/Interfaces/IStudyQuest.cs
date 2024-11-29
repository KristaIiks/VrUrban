namespace StudySystem
{
	public interface IStudyQuest: IStudyStart, IStudyComplete
	{
		void Restart();
	}
}