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
                RaycastHit hit;
                bool _hasHit = Physics.Raycast(transform.position, transform.forward, out hit, _rayDistance, _rayMask);

                if (_hasHit && (_currentRayObject == null || hit.collider.gameObject != _currentRayObject))
                {
                    _currentRayObject = hit.collider.GetComponentInParent<Selectable>();
                    SetRayColor(_currentRayObject._canInteract ? CanSelectGrad : ErrorSelectGrad);
                }
                else if (_selectAction.action.IsPressed())
                {
                    SetRayColor(ErrorSelectGrad);
                }
                else if (!_hasHit)
                {
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
