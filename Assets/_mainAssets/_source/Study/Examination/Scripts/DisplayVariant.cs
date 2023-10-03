using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Study.Examination
{
    public class DisplayVariant : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;
        [SerializeField] private Image _changeColor;

        private bool _isInit = false;

        public void Init(string text, uint id)
        {
            if (_isInit) { return; }

            _text.text = text;
            _button.onClick.AddListener(() => Interact(id));

            _isInit = true;
        }

        private void Interact(uint id)
        {
            if (Examination.Instance.TryAnswer(id))
            {
                _changeColor.color = Color.green;
            }
            else
            {
                _changeColor.color = Color.red;
            }
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
