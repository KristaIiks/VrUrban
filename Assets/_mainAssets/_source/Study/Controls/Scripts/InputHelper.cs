using UnityEngine;
using UnityEngine.InputSystem;

namespace Study.Controls
{
    // TODO: add other controllers
    public class InputHelper : MonoBehaviour
    {
        [SerializeField] private GameObject _handModel;
        [SerializeField] private GameObject _controllerModel;

        [SerializeField] private Material _newMat;

        [SerializeField] private ControllerTips[] _tips;

        private Renderer _current;
        private InputAction _action;
        private Material _oldMat;
        private bool _activeState = false;

        public void Study(int _id)
        {
            ControllerBtn _btn = (ControllerBtn)_id;
            bool _autoFinish = true;
            StopStudy();

            _activeState = _handModel.activeSelf;
            _handModel.SetActive(false);
            _controllerModel.SetActive(true);

            foreach (ControllerTips item in _tips)
            {
                if (item._btn == _btn)
                {
                    _current = item._model.GetComponentInChildren<Renderer>();
                    _oldMat = _current.material;
                    _current.material = _newMat;

                    if (_autoFinish)
                    {
                        _action = item._action;
                        _action.performed += StopStudy;
                    }
                    break;
                }
            }

        }

        public void StopStudy(InputAction.CallbackContext cnt = new InputAction.CallbackContext())
        {
            if(_current == null) { return; }

            _handModel.SetActive(_activeState);
            _controllerModel.SetActive(false);

            _current.material = _oldMat;

            _action.performed -= StopStudy;
            _action = null;
            _current = null;
            _oldMat = null;
        }

    }

    [System.Serializable]
    public struct ControllerTips
    {
        public GameObject _model;
        public ControllerBtn _btn;
        public InputActionReference _action;
    }

    [System.Serializable]
    public enum ControllerBtn
    {
        Trigger,
        Joystick,
        Grip,
        Menu,
        None
    }
}
