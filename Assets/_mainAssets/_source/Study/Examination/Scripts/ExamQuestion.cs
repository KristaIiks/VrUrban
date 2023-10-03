using UnityEngine;

namespace Study.Examination
{
    [CreateAssetMenu(fileName = "New Question", menuName = "Study/Examination/New Question")]
    public class ExamQuestion : ScriptableObject
    {
        //[Range(1, 10)]
        //public int _questiontLevel = 1;

        public Question[] _variants;

        public Question GenerateQuestion()
        {
            return _variants[Random.Range(0, _variants.Length - 1)];
        }
    }
}
