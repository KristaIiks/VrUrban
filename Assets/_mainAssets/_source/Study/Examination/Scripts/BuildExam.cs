using UnityEngine;
using UnityEngine.UI;

namespace Study.Examination
{
    public class BuildExam : MonoBehaviour
    {
        [SerializeField] private Button _btn;

        [SerializeField] private Transform _thirstSlot;
        private bool _one;
        [SerializeField] private Transform _secondSlot;
        private bool _two;

        [SerializeField] private string[] _correctObj;

        [SerializeField] private uint _maxAttemps = 3;

        private uint _currentAttemps;

        private void Awake()
        {
            _currentAttemps = _maxAttemps;
            _btn.onClick.AddListener(Try);
        }

        private void OnDestroy()
        {
            _btn.onClick.RemoveListener(Try);
        }

        public void SelectBuild(string _model)
        {
            if (!_one)
            {
                foreach (GameObject item in _thirstSlot)
                {
                    if (item.name == _model)
                    {
                        item.SetActive(true);
                        _one = true;
                        return;
                    }
                }
            }
            else if (!_two)
            {
                foreach (GameObject item in _secondSlot)
                {
                    if (item.name == _model)
                    {
                        item.SetActive(true);
                        _two = true;
                        return;
                    }
                }
            }
        }

        public void DeselectBuild(GameObject _model)
        {
            foreach (GameObject item in _thirstSlot)
            {
                if (item == _model)
                {
                    item.SetActive(false);
                    _one = false;
                    return;
                }
            }

            foreach (GameObject item in _secondSlot)
            {
                if (item == _model)
                {
                    item.SetActive(false);
                    _two = false;
                    return;
                }
            }
        }

        public void Try()
        {
            _currentAttemps--;

            foreach (var item in _correctObj)
            {
                foreach (GameObject slot1 in _thirstSlot)
                {
                    if (slot1.name == item && !slot1.activeSelf)
                    {
                        if (_currentAttemps == 0)
                        {
                            Examination.Instance.CompleteAndStart(_maxAttemps, false);
                            _btn.onClick.RemoveListener(Try);
                        }

                        return;
                    }
                }

                foreach (GameObject slot2 in _thirstSlot)
                {
                    if (slot2.name == item && !slot2.activeSelf)
                    {
                        if (_currentAttemps == 0)
                        {
                            Examination.Instance.CompleteAndStart(_maxAttemps, false);
                            _btn.onClick.RemoveListener(Try);
                        }

                        return;
                    }
                }
            }

            Examination.Instance.CompleteAndStart(_maxAttemps - _currentAttemps, true);
            _btn.onClick.RemoveListener(Try);
        }
    }
}
