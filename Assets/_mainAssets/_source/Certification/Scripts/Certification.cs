using UnityEngine;
using UnityEngine.Events;

namespace Certification
{
    public class Certification
    {
        public UnityAction<ÑertificationResult> CertificationResult;

        private TestScriptable CurrentTest;

        private int _currentQuestion = 0;

        private int _attemps = 0;
        private int _totalErrors = 0;

        public Certification(TestScriptable _test)
        {
            CurrentTest = _test;
        }

        public bool SetAnswer(int _answer)
        {
            if (_currentQuestion >= CurrentTest.Questions.Length ||
                _answer < 0 ||
                _answer > CurrentTest.Questions[_currentQuestion].Answers.Length)
                return false;

            _attemps++;

            if (!CurrentTest.Questions[_currentQuestion].Answers[_answer]._isAnswer)
            {
                _totalErrors++;

                return false;
            }

            if (_currentQuestion < CurrentTest.Questions.Length)
            {
                _currentQuestion++;
            }
            else
            {
                CertificationResult?.Invoke(new ÑertificationResult(CurrentTest.Questions.Length + 1, _attemps, _totalErrors));
            }

            return true;
        }
    }

    public struct ÑertificationResult
    {
        public float QuestionsCount { get; }
        public float Attemps { get; }
        public float Errors { get; }

        public float Result { get => Mathf.Abs((1 - Errors / Attemps) * 100); }

        public ÑertificationResult(int _questions, int _attemps, int _errors)
        {
            QuestionsCount = _questions;
            Attemps = _attemps;
            Errors = _errors;
        }
    }

}
