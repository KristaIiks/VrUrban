using UnityEngine;

namespace Study.Examination
{
    [System.Serializable]
    public class QuestionVariant
    {
        //[field: Range(1, 10)]
        //[field: SerializeField] public int _variantLevel { get; private set; }

        [field: SerializeField] public Answers[] Answers { get; private set; }

        public bool TryAnswer(uint _id) => Answers[_id]._isAnswer;
    }

    [System.Serializable]
    public struct Answers
    {
        public string _text;
        public Sprite _sprite;

        public AudioClip _reaction;
        public bool _isAnswer;
    }
}
