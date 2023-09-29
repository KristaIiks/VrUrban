using System;
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

        public QuestionVariant GenerateQuestion() => _variants[UnityEngine.Random.Range(0, _variants.Length - 1)];
    }

    public abstract class QuestionVariant : ScriptableObject
    {
        [field: Range(1, 10)]
        [field: SerializeField] public int _variantLevel { get; private set; }

        [field: SerializeField] public string _question { get; private set; }
        [field: SerializeField] public AudioClip _audio { get; private set; }

        [SerializeField] private string _help;

        public void GetHelp()
        {
            Debug.Log(_help); // Test help
        }
    }

    public struct QuestionResult
    {
        public float _time;
        public bool _result;
        public string _comments;

        public QuestionResult(float time, bool result, string comments = "")
        {
            _time = time;
            _result = result;
            _comments = comments;
        }
    }

    [CreateAssetMenu(fileName = "Standart", menuName = "Study/Examination/New Question Variant/Standart")]
    public class StandartQuestion : QuestionVariant
    {
        public Answers[] Answers;
    }

    // Sample how use custom Question Variant
    //
    //[CreateAssetMenu(fileName = "Special", menuName = "Study/Examination/New Question Variant/Special")]
    //public class SpecialQuestion : QuestionVariant
    //{
    //    public string _test;
    //}

    [System.Serializable]
    public struct Answers
    {
        public string _text;
        public Sprite _image;

        public AudioClip _reaction;

        public bool _isAnswer;
    }
}
