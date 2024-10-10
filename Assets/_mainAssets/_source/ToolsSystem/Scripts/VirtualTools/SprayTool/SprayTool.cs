using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace ToolsSystem
{
	
	// TODO: replace to Physic Tool
	// TODO: add filter points (grass, graffiti or other)
	public sealed class SprayTool : VirtualTool
	{
		[SerializeField] private InputActionReference _btnInput;
		[SerializeField] private float _sprayDistance = 2f;
		[SerializeField] private float _interactionDelay = .5f;
		[SerializeField] private VisualEffect _effect;
		
		private float _lastTime;
		private bool _isWork;
		
		private void OnEnable()
		{
			_btnInput.action.performed += OnInteractTool;
		}

		private void OnDisable()
		{
			_btnInput.action.performed -= OnInteractTool;
		}
		
		private void Update()
		{
			if (!IsEnabled || !_isWork) { return; }
			
			_lastTime += Time.deltaTime;

			if (_lastTime >= _interactionDelay)
			{
				_lastTime = 0f;
				
				TryInteract();
			}	
		}
		
		private void TryInteract()
		{
			if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _sprayDistance) && hit.collider.tag == SprayPoint.GRASS_TAG)
			{
				hit.transform.GetComponent<SprayPoint>()?.Paint();
			}
		}
		
		private void OnInteractTool(InputAction.CallbackContext cnt)
		{
			if (cnt.started)
			{
				_effect.Play();
				_audio.Play();
				_isWork = true;
			}
			else if (cnt.canceled)
			{
				_effect.Stop();
				_audio.Stop();
				_isWork = true;
			}
		}
	}
}
