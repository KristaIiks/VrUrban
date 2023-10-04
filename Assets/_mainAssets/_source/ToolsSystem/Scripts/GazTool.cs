using UnityEngine;
using UnityEngine.InputSystem;

namespace ToolsSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class GazTool : Tool
    {
        [SerializeField] private InputActionReference _input;
        private bool _isEnabled = false;

        [SerializeField] private ParticleSystem _effect;

        public static GazTool Instance;

        private void OnValidate()
        {
            _source ??= GetComponent<AudioSource>();
        }

        protected override void Awake()
        {
            base.Awake();

            Instance = this;

            _input.action.performed += (s) => _isEnabled = true;
            _input.action.canceled += (s) => _isEnabled = false;
        }

        protected override void Start() { } // No ui

        private float _lastTime = 0;
        protected override void Update()
        {
            _lastTime += Time.deltaTime;

            if (_isEnabled)
            {
                if (!_source.isPlaying)
                {
                    _effect.Play();
                    _source.Play();

                    _lastTime = 0f;
                }

                if (_lastTime >= .2f)
                {
                    _lastTime = 0f;

                    RaycastHit _hit;
                    PaintPoint _point;

                    if (Physics.Raycast(transform.position, Vector3.forward, out _hit, 5f) && _hit.transform.TryGetComponent(out _point))
                    {
                        _point.Paint();
                    }
                }
            }

            if (_source.isPlaying)
            {
                _effect.Stop();
                _source.Stop();
            }
        }
    }
}
