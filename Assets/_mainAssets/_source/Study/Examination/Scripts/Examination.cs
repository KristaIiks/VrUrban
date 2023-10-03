using StudySystem;
using UnityEngine;

namespace Study.Examination
{
    public class Examination: MonoBehaviour
    {
        [SerializeField] private ExamDisplay _display;

        public static Examination Instance { get; private set; }

        private Question[] _currentExam = null;
        private uint _currentQuestion = 0;

        public uint _totalAttemps = 0;

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
            string[] _variants = new string[_currentExam[_currentQuestion]._variant.Answers.Length];

            for (int i = 0; i < _currentExam[_currentQuestion]._variant.Answers.Length; i++)
            {
                _variants.SetValue(_currentExam[_currentQuestion]._variant.Answers[i]._text + "\n", i);
            }

            _display.Display(_currentExam[_currentQuestion]._question, _variants);
        }

        public bool TryAnswer(uint _id)
        {
            _totalAttemps++;

            bool _result = _currentExam[_currentQuestion]._variant.TryAnswer(_id);

            if (_result)
            {
                Invoke("StartQuestion", 2f);
            }

            StudyManager.Instance.Sound(_currentExam[_currentQuestion]._variant.Answers[_id]._reaction);

            return _result;
        }

        public void StartQuestion()
        {
            _currentQuestion++;

            if(_currentQuestion >= _currentExam.Length) { _display.Clear(); return; } // finish

            if (_currentExam[_currentQuestion]._custom != null)
            {
                _display.Clear();
                _display.Display(_currentExam[_currentQuestion]._question);

                // Spawn quest

                return;
            }

            string[] _variants = new string[_currentExam[_currentQuestion]._variant.Answers.Length];

            for (int i = 0; i < _currentExam[_currentQuestion]._variant.Answers.Length; i++)
            {
                _variants.SetValue(_currentExam[_currentQuestion]._variant.Answers[i]._text + "\n", i);
            }

            _display.Display(_currentExam[_currentQuestion]._question, _variants);
        }
    }
}