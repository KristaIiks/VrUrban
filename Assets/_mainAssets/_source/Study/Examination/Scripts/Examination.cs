using UnityEngine;

namespace Study.Examination
{
    public class Examination: MonoBehaviour
    {
        private QuestionVariant[] _currentExam = null;

        public void StartExam(ExamScriptable _exam)
        {
            if(_currentExam != null) { return; }

            // Generate questions
            _currentExam = _exam.GenerateExam();

            // Prepare for start
            PrepareGUI();

            // Start first question
            //
        }

        private void PrepareGUI()
        { 

        }
    }
}
