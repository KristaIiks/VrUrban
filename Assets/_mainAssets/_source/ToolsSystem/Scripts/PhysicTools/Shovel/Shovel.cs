using System;
using UnityEngine;
using UnityEngine.VFX;
using SmartConsole;

namespace ToolsSystem
{
    // TODO: сбросить кучку
    public sealed class Shovel : PhysicTool
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private CrumblyBlockSettings _defaultSettings;
        [SerializeField] private Transform _effectsTransform;
        [SerializeField] private MeshRenderer _hillObject;
        
        public event Action<CrumblyBlock> OnDig;
        
        private bool _isFilled;
        private float _angle = 0.5f;  

        private void Awake() => Reset();
		private void Update()
        {
			if (_isFilled && CheckRotation(-_angle))
			{
				ActivateHill(false);
			}
		}
        

        protected override void SelectTool(bool state)
        {
            base.SelectTool(state);
            _collider.isTrigger = state;
        }

        private void DigObject(CrumblyBlock block)
        {
            if (block == null && CheckRotation(_angle)) { return; }
            
            SConsole.Log(LOG_TAG, $"Попытка копать - {block.gameObject.name}");
            if(block.Dig(out CrumblyBlockSettings settings))
            {
                _audio.PlayOneShot(settings.DigSound ?? _defaultSettings.DigSound);
            
                VisualEffect vfx = Instantiate(
                    settings.DigEffect ?? _defaultSettings.DigEffect,
                    _effectsTransform.position, 
                    Quaternion.identity, 
                    transform
                );
                Destroy(vfx, 3f);
                
                _hillObject.material = settings.Material ?? _defaultSettings.Material;
                ActivateHill(true);
                
                OnDig?.Invoke(block);
                SConsole.Log(LOG_TAG, $"Успешное копание");
            }
        }

        private void ActivateHill(bool state)
        {
            _isFilled = state;
            _hillObject.gameObject.SetActive(_isFilled);
        }

        private void Reset()
        {
            ActivateHill(false);
            _collider.isTrigger = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isGrabbed && !_isFilled && other.TryGetComponent(out CrumblyBlock block))
            {
                DigObject(block);
            }
        }
		private bool CheckRotation(float angle)
        {
            float dotProduct = Vector3.Dot(transform.up, Vector3.up);

            return dotProduct < angle;
        }
        
    }
}
