using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace ToolsSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class GazTool : Tool
    {
        [SerializeField] private InputActionReference _input;
        private bool _isEnabled = false;

        [SerializeField] private VisualEffect _effect;

        public static GazTool Instance;

        private void OnValidate()
        {
            _source ??= GetComponent<AudioSource>();
        }

        protected override void Awake()
        {
            base.Awake();

            _input.action.performed += (s) => _isEnabled = true;
            _input.action.canceled += (s) => _isEnabled = false;

            Instance ??= this;
            gameObject.SetActive(false);
        }

        protected override void Start() { } // No ui

        private float _lastTime = 0;
        protected override void Update()
        {
            if (_isEnabled)
            {
                _lastTime += Time.deltaTime;

                if (!_source.isPlaying)
                {
                    _effect.Play();
                    _source.Play();
                }

                if (_lastTime >= .5f)
                {
                    _lastTime = 0f;
                    
                    RaycastHit _hit;
                    PaintPoint _point;

                    if (Physics.Raycast(transform.position, transform.forward, out _hit, 2f) && _hit.transform.TryGetComponent(out _point))
                    {
                        _point.Paint();
                    }
                }
            }
            else
            {
                _effect.Stop();
                _source.Stop();
            }

            
        }
    }
}
