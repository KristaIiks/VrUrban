using System;
using SmartConsole;
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
		
		[Space(25)]
		[SerializeField, Min(.1f)] private float _sprayDistance = 2f;
		[SerializeField] private float _interactionDelay = .5f;
		
		[Space(25)]
		[SerializeField] private AudioClip _sprayClip;
		[SerializeField] private VisualEffect _effect;
		
		public event Action<SprayPoint> PaintPoint;
		
		private float _lastTime;
		private bool _isWork;

		protected override void OnValidate()
		{
			base.OnValidate();
			_audio.clip = _sprayClip;
		}

		private void Awake() => ChangeInputState(false);
		
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
			if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _sprayDistance) 
			&& hit.collider.TryGetComponent(out SprayPoint sprayPoint))
			{
				SConsole.Log(LOG_TAG, $"Try paint - {sprayPoint.gameObject.name}");
				if (sprayPoint.Paint())
				{
					PaintPoint?.Invoke(sprayPoint);
					SConsole.Log(LOG_TAG, "Successful pained");
				}
			}
		}

		protected override void SelectTool(bool state)
		{			
			if (state) 
			{
				_interactInput.action.started += OnInteractTool;
				_interactInput.action.canceled += OnInteractTool;
			}
			else
			{
				_interactInput.action.started -= OnInteractTool;
				_interactInput.action.canceled -= OnInteractTool;
				
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
				_lastTime = 0f;
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

		private void OnDrawGizmosSelected()
		{
			if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _sprayDistance)
			&& hit.collider.TryGetComponent(out SprayPoint sprayPoint))
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(transform.position, hit.point);
				return;
			}
			
			Gizmos.color = Color.red;
			Gizmos.DrawRay(transform.position, transform.forward * _sprayDistance);
		}
	}
}
