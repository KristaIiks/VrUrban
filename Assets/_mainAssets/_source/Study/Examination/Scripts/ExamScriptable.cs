using UnityEngine;

namespace Study.Examination
{
    [CreateAssetMenu(fileName = "New Exam", menuName = "Study/Examination/New Exam")]
    public class ExamScriptable : ScriptableObject
    {
        public ExamQuestion[] _questions;

        public QuestionVariant[] GenerateExam()
        {
            if(_questions.Length == 0) { return null; }

            QuestionVariant[] _exam = new QuestionVariant[_questions.Length];

            for (int i = 0; i < _questions.Length; i++)
            {
                _exam[i] = _questions[i].GenerateQuestion();
            }
            return _exam;
        }
    }

    [CreateAssetMenu(fileName = "New Question", menuName = "Study/Examination/New Question")]
    public class ExamQuestion : ScriptableObject
    {
        [Range(1, 10)]
        public int _questiontLevel = 1;

        public QuestionVariant[] _variants;

        public QuestionVariant GenerateQuestion() => _variants[Random.Range(0, _variants.Length - 1)];
    }

    public abstract class QuestionVariant : ScriptableObject
    {
        [field: Range(1, 10)]
        [field: SerializeField] public int _variantLevel { get; private set; }

        [field: SerializeField] public string _question { get; private set; }
        [field: SerializeField] public AudioClip _audio { get; private set; }

        [field: SerializeField] public Answers[] Answers { get; private set; }

        public bool TryAnswer(uint _id) => Answers[_id]._isAnswer;
    }

    [System.Serializable]
    public struct Answers
    {
        public string _text;

        public AudioClip _reaction;
        public bool _isAnswer;
    }
}
