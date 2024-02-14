using StudySystem;
using TMPro;
using UnityEngine;

namespace Study.Examination
{
    public class Examination: MonoBehaviour
    {
        [SerializeField] private ExamDisplay _display;
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private Transform _table;
        [SerializeField] private GameObject _text;

        [SerializeField] private AudioSource _answerSource;

        [SerializeField] private AudioClip _correctAnswer;
        [SerializeField] private AudioClip _wrongAnswer;

        public static Examination Instance { get; private set; }

        private Question[] _currentExam = null;
        private uint _currentQuestion = 0;

        private uint _totalAttemps = 0;
        private uint _correctAttemps = 0;

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
            Sprite[] _imgs = new Sprite[_currentExam[_currentQuestion]._variant.Answers.Length];

            for (int i = 0; i < _currentExam[_currentQuestion]._variant.Answers.Length; i++)
            {
                _variants.SetValue(_currentExam[_currentQuestion]._variant.Answers[i]._text + "\n", i);
                _imgs[i] = _currentExam[_currentQuestion]._variant.Answers[i]._sprite;
            }

            _display.Display(_currentExam[_currentQuestion]._question, _variants, _imgs);
        }

        [HideInInspector]
        public bool _hasCorrect = false;
        public bool TryAnswer(uint _id)
        {
            _totalAttemps++;

            bool _result = _currentExam[_currentQuestion]._variant.TryAnswer(_id);
            StudyManager.Instance.Sound(_result ? _correctAnswer : _wrongAnswer);

            if (_result)
            {
                Invoke("StartQuestion", 2f);
                _correctAttemps++;
                _hasCorrect = true;
            }

            _answerSource.Stop();
            _answerSource.PlayOneShot(_currentExam[_currentQuestion]._variant.Answers[_id]._reaction);

            return _result;
        }

        public void Answer(uint _id)
        {
            _totalAttemps++;
            _hasCorrect = true;

            bool _result = _currentExam[_currentQuestion]._variant.TryAnswer(_id);
            StudyManager.Instance.Sound(_result ? _correctAnswer : _wrongAnswer);

            if (_result)
            {
                _correctAttemps++;
            }

            _answerSource.Stop();
            _answerSource.PlayOneShot(_currentExam[_currentQuestion]._variant.Answers[_id]._reaction);
        }

        public void CompleteAndStart(uint _attemps, bool _hasCorrect)
        {
            _totalAttemps++;
            if(_hasCorrect)
            {
                _correctAttemps++;
            }
            Invoke("StartQuestion", 2f);
        }

        private void StartQuestion()
        {
            _hasCorrect = false;
            _currentQuestion++;

            if(_currentQuestion == 2)
            {
                _text.SetActive(true);
            }

            if(_currentQuestion >= _currentExam.Length) { _display.Clear(); _resultText.text = Mathf.FloorToInt((float)_correctAttemps / (float)_totalAttemps * 100).ToString() + "%"; return; } // finish

            if (_currentExam[_currentQuestion]._custom != null)
            {
                _display.Clear();
                _display.Display(_currentExam[_currentQuestion]._question);

                Instantiate(_currentExam[_currentQuestion]._custom, _table);

                return;
            }

            string[] _variants = new string[_currentExam[_currentQuestion]._variant.Answers.Length];
            Sprite[] _imgs = new Sprite[_currentExam[_currentQuestion]._variant.Answers.Length];

            for (int i = 0; i < _currentExam[_currentQuestion]._variant.Answers.Length; i++)
            {
                _variants.SetValue(_currentExam[_currentQuestion]._variant.Answers[i]._text + "\n", i);
                _imgs[i] = _currentExam[_currentQuestion]._variant.Answers[i]._sprite;
            }

            _display.Display(_currentExam[_currentQuestion]._question, _variants, _imgs);
        }
    }
}