using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Hands
{
    public class HandControls : MonoBehaviour
    {
        [SerializeField] private GameObject _standartHand;

        [SerializeField] private GameObject _teleportHand;
        [SerializeField] private InputActionReference _teleportAction;

        [SerializeField] private ControllerTips[] _tips;
        private GameObject _current;


        private void Awake()
        {
            _teleportAction.action.performed += StartTeleport;
            _teleportAction.action.canceled += FinishTeleport;
        }

        private void StartTeleport(InputAction.CallbackContext cnt)
        {
            Debug.Log("+");
            _standartHand.SetActive(false);
            _teleportHand.SetActive(true);
        }

        private void FinishTeleport(InputAction.CallbackContext cnt) => Invoke("Deactivate", .1f);
        private void Deactivate()
        {
            Debug.Log("-");
            _standartHand.SetActive(true);
            _teleportHand.SetActive(false);
        }

        #region Study

        public void StartStudy(ControllerBtn _btn) => Study(_btn);
        public void StartStudy(ControllerBtn _btn, bool _autoFinish) => Study(_btn, _autoFinish);

        private void Study(ControllerBtn _btn, bool _autoFinish = true)
        {
            foreach (ControllerTips item in _tips)
            {
                if (item._btn == _btn)
                {
                    _current = item._model;

                    //TODO: action with object

                    if (_autoFinish)
                    {
                        item._action.performed += (s) => StopStudy(_btn); //TODO: action unsubscribe
                    }
                    break;
                }
            }

        }

        public void StopStudy(ControllerBtn _btn = ControllerBtn.None)
        {
            if (_btn != ControllerBtn.None) // Auto finish
            {
                foreach (ControllerTips item in _tips)
                {
                    if (item._btn == _btn)
                    {
                        item._model.SetActive(false);
                        break;
                    }
                }

                return;
            }
            _current?.SetActive(false);
        }

        #endregion

    }
}

[System.Serializable]
public struct ControllerTips
{
    public GameObject _model;
    public ControllerBtn _btn;
    public InputAction _action;
}

[System.Serializable]
public enum ControllerBtn
{
    Trigger,
    Joystick,
    Catch,
    Menu,
    None
}
