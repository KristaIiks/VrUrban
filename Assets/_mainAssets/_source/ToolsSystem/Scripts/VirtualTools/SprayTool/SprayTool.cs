using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace ToolsSystem
{
	
	// TODO: replace to Physic Tool
	// TODO: add filter points (grass, graffiti or other)
	public sealed class SprayTool : VirtualTool
	{
		[SerializeField] private InputActionReference _interactInput;
		[SerializeField, Min(.1f)] private float _sprayDistance = 2f;
		[SerializeField] private float _interactionDelay = .5f;
		[SerializeField] private VisualEffect _effect;
		
		private float _lastTime;
		private bool _isWork;
		
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
			if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _sprayDistance) && hit.collider.tag == SprayPoint.SPRAY_POINT_TAG)
			{
				hit.transform.GetComponent<SprayPoint>()?.Paint();
			}
		}

		protected override void SelectTool(bool state)
		{			
			if (state) 
			{
				_interactInput.action.performed += OnInteractTool;
			}
			else
			{
				_interactInput.action.performed -= OnInteractTool;
				ChangeInputState(state);
			}
			
			base.SelectTool(state);
		}

		private void ChangeInputState(bool state)
		{
			if (state)
			{
				_effect.Play();
				_audio.Play();
				_isWork = true;
			}
			else
			{
				_effect.Stop();
				_audio.Stop();
				_isWork = false;
			}
		}
		
		private void OnInteractTool(InputAction.CallbackContext cnt)
		{
			if (cnt.started)
			{
				ChangeInputState(true);
			}
			else if (cnt.canceled)
			{
				ChangeInputState(false);
			}
		}
	}
}
