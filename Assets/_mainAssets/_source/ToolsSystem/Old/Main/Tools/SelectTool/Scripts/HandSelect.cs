using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ToolsSystem.Old
{
    [RequireComponent(typeof(LineRenderer), typeof(AudioSource))]
    public class HandSelect : MonoBehaviour
    {
        [SerializeField] private Transform RayPoint;
        [SerializeField] private float RayDistance = 10f;
        [SerializeField] private LayerMask RayMask;
        [SerializeField] private InputActionReference SelectAction;
        [SerializeField] private AudioClip OnSelectClip;

        private Selectable _currentObject;

        public static HandSelect Instance;
        public static UnityEvent OnSelect = new UnityEvent();
        public static UnityEvent OnDeselect = new UnityEvent();

        private LineRenderer _line;
        private AudioSource _audioSource;

        private void Awake()
        {
            SelectAction.action.performed += UpdateSelection;
            _line = GetComponent<LineRenderer>();
            _audioSource = GetComponent<AudioSource>();

            if (Instance == null) { Instance = this; }
        }

        private void Update()
        {
            RaycastHit hit;

            if (Physics.Raycast(RayPoint.position, RayPoint.forward, out hit, RayDistance, RayMask))
            {
                _line.startColor = Color.green;
                _line.enabled = true;

                _line.SetPosition(0, RayPoint.position);
                _line.SetPosition(1, hit.point);
            }
            else if (SelectAction.action.IsPressed())
            {
                _line.startColor = Color.red;
                _line.enabled = true;

                _line.SetPosition(0, RayPoint.position);
                _line.SetPosition(1, RayPoint.position + RayPoint.forward * RayDistance);
            }
            else
            {
                _line.enabled = false;
            }
        }

        public void Deselect()
        {
            if (_currentObject != null)
            {
                OnDeselect?.Invoke();
                _currentObject.Deselect();
                _currentObject = null;
            }
        }

        private void UpdateSelection(InputAction.CallbackContext cont)
        {
            RaycastHit hit;

            if (Physics.Raycast(RayPoint.position, RayPoint.forward, out hit, RayDistance, RayMask))
            {
                Selectable _tmp = hit.collider.GetComponentInParent<Selectable>();

                if (_tmp.CanInteract())
                {
                    Deselect();
                    _currentObject = _tmp;

                    _currentObject.Interact();
                    _audioSource.PlayOneShot(OnSelectClip);
                    OnSelect?.Invoke();
                }
            }

        }

    }
}
