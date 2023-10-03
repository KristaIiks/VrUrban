using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Study.Examination
{
    public class DisplayVariant : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;

        private bool _isInit = false;

        public void Init(string text, int id)
        {
            if (_isInit) { return; }

            _text.text = text;
            //_button.onClick.AddListener(); Change btn color

            _isInit = true;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
