using UnityEngine;

namespace Study.Examination
{
    public class Examination: MonoBehaviour
    {
        [SerializeField] private ExamDisplay _display;

        public static Examination Instance { get; private set; }

        private QuestionVariant[] _currentExam = null;
        private uint _currentQuestion = 0;

        private uint _totalAttemps = 0;

        private void OnValidate()
        {
            Instance = this;
        }

        public void StartExam(ExamScriptable _exam)
        {
            if(_currentExam != null) { return; }

            // Generate questions
            _currentExam = _exam.GenerateExam();

            // Start first question
            StartQuestion();
        }

        public bool TryAnswer(uint _id)
        {
            _totalAttemps++;

            bool _result = _currentExam[_currentQuestion].TryAnswer(_id);

            if (_result)
            {
                // Start next
            }

            return _result;
        }

        public void StartQuestion()
        {
            string[] _variants = new string[_currentExam[_currentQuestion].Answers.Length];

            for (int i = 0; i < _currentExam[_currentQuestion].Answers.Length; i++)
            {
                _variants.SetValue(_currentExam[_currentQuestion].Answers[i]._text + "\n", i);
            }

            _display.Display(_currentExam[_currentQuestion]._question, _variants);

            _currentQuestion++;
        }
    }
}