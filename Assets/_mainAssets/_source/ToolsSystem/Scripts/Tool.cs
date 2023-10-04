using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ToolsSystem
{
    public abstract class Tool : MonoBehaviour
    {
        [SerializeField] private string _toolName;

        #region Ray select

        [SerializeField] private bool _canSelect = true;

        [SerializeField] private GameObject _uiHand;

        [SerializeField] private Transform _transform;
        [SerializeField] private float _rayDistance = 10f;
        [SerializeField] private LayerMask _rayMask;

        [SerializeField] private Gradient CanSelectGrad;
        [SerializeField] protected Gradient ErrorSelectGrad;

        [SerializeField] private InputActionReference _selectAction;
        [SerializeField] private AudioClip _selectClip;

        public static UnityEvent OnSelect = new UnityEvent();
        public static UnityEvent OnDeselect = new UnityEvent();

        private LineRenderer _line;
        protected AudioSource _source;

        #endregion

        protected ToolWindow _window;
        protected Selectable _selectedObject;

        protected virtual void Awake()
        {
            if(_canSelect)
            {
                _line = GetComponent<LineRenderer>();
                _source = GetComponent<AudioSource>();

                _selectAction.action.performed += (s) => Interact();
            }
        }

        protected virtual void Start()
        {
            _window = ToolsManager.Instance.GetWindow(_toolName);
        }

        Selectable _currentRayObject;
        protected virtual void Update()
        {
            if (_canSelect)
            {
                _line.SetPosition(0, _transform.position);

                RaycastHit hit;
                bool _hasHit = Physics.Raycast(_transform.position, _transform.forward, out hit, _rayDistance, _rayMask);

                if (_hasHit)
                {
                    Transform _obj = hit.transform;

                    while (_obj.parent != null)
                    {
                        if (_obj.parent.tag == "SelectObject")
                        {
                            _obj = _obj.parent;
                            break;
                        }
                        _obj = _obj.parent;
                    }
                    _line.SetPosition(1, hit.point);

                    if (_currentRayObject == null || _obj.gameObject != _currentRayObject.gameObject)
                    {
                        _currentRayObject = _obj.GetComponent<Selectable>();

                        SetRayColor(_currentRayObject._canInteract ? CanSelectGrad : ErrorSelectGrad);

                        _uiHand.SetActive(false);
                        _line.enabled = true;
                    }
                }
                else if (_selectAction.action.IsPressed())
                {
                    _currentRayObject = null;

                    SetRayColor(ErrorSelectGrad);
                    _line.SetPosition(1, _transform.position + _transform.forward * _rayDistance);

                    _uiHand.SetActive(false);
                    _line.enabled = true;
                }
                else if (!_hasHit)
                {
                    _currentRayObject = null;

                    _uiHand.SetActive(true);
                    _line.enabled = false;
                }
            }
        }

        private void SetRayColor(Gradient _gradient) => _line.colorGradient = _gradient;

        protected virtual void Interact()
        {
            if (_currentRayObject == null || !_currentRayObject._canInteract) { return; }

            _currentRayObject.Interact();

            if (_selectedObject != _currentRayObject)
            {
                SelectObj(_currentRayObject._params);
            }
        }

        protected virtual void SelectObj(ToolParams _params)
        {
            OpenMenu(_params);
            _source.PlayOneShot(_selectClip);

            if(_selectedObject != null)
            {
                DeselectObj(_selectedObject);
            }

            _selectedObject = _currentRayObject;
            _selectedObject.Select();

            OnSelect?.Invoke();
        }

        protected virtual void DeselectObj(Selectable _obj)
        {
            _obj.Deselect();

            OnDeselect?.Invoke();
        }

        #region Menu

        protected virtual void OpenMenu(ToolParams _params) => _window.Open(_params);
        public virtual void CloseMenu()
        {
            _window.Close();

            if(_selectedObject != null)
            {
                DeselectObj(_selectedObject);
                _selectedObject = null;
            }
        }

        #endregion

    }
}
