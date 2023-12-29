using UnityEngine;

namespace Study.Examination
{
    [CreateAssetMenu(fileName = "New Exam", menuName = "Study/Examination/New Exam")]
    public class ExamScriptable : ScriptableObject
    {
        public ExamQuestion[] _questions;

        public Question[] GenerateExam()
        {
            if(_questions.Length == 0) { return null; }

            Question[] _exam = new Question[_questions.Length];

            for (int i = 0; i < _questions.Length; i++)
            {
                _exam[i] = _questions[i].GenerateQuestion();
            }
            return _exam;
        }
    }

    [System.Serializable]
    public struct Question
    {
        [field: SerializeField] public string _question { get; private set; }
        [field: SerializeField] public AudioClip _audio { get; private set; }

        public QuestionVariant _variant;
        public GameObject _custom;
    }
}
