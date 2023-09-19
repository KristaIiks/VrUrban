using UnityEngine;

namespace Certification
{
    [CreateAssetMenu(fileName = "NewCertification", menuName = "Study/Sertification")]
    public class TestScriptable : ScriptableObject
    {
        public Questions[] Questions;
    }

    [System.Serializable]
    public struct Questions
    {
        public string Question;
        public Answers[] Answers;
    }

    [System.Serializable]
    public struct Answers
    {
        public string Text;
        public Sprite Image;
        public AudioClip Reaction;

        public bool _isAnswer;
    }
}
