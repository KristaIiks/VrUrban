using TMPro;
using UnityEngine;

namespace Study.Examination
{
    public class ExamDisplay: MonoBehaviour
    {
        [SerializeField] private TMP_Text _questionText;

        [SerializeField] private Transform _content;
        [SerializeField] private DisplayVariant _standartPrefab;

        public void Display(string _question)
        {
            _questionText.text = _question;
        }

        public void Display(string _question, string[] _variants, Sprite[] _imgs = null, DisplayVariant _variantPrefab = null)
        {
            Clear();

            _questionText.text = _question;

            if (_imgs == null)
            {
                _imgs = new Sprite[_variants.Length];
            }

            if (_variantPrefab == null)
            {
                _variantPrefab = _standartPrefab;
            }

            for (uint i = 0; i < _variants.Length; i++)
            {
                DisplayVariant _obj = Instantiate(_variantPrefab, _content);
                _obj.Init(_variants[i], i, _imgs[i]);
            }
        }

        public void Clear()
        {
            foreach (Transform item in _content)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
