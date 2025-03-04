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
		[SerializeField] private InputActionReference Input;
		
		[Space(25)]
		[SerializeField, Min(.1f)] private float Distance = 2f;
		[SerializeField] private float InteractDelay = .5f;
		
		[Space(25)]
		[SerializeField] private VisualEffect VFX;
		
		public event Action<SprayPoint> Paint;
		
		private float _lastTime;
		private bool _isWork;

		private void Awake() => ChangeInputState(false);
		
		private void Update()
		{
			if (!IsEnabled || !_isWork) { return; }
			
			_lastTime += Time.deltaTime;

			if (_lastTime >= InteractDelay)
			{
				_lastTime = 0f;
				
				Interact();
			}	
		}
		
		private void Interact()
		{
			if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Distance) 
			&& hit.collider.TryGetComponent(out SprayPoint sprayPoint))
			{
				SConsole.Log(LOG_TAG, $"Try paint - {sprayPoint.gameObject.name}");
				
				if (sprayPoint.Paint())
				{
					Paint?.Invoke(sprayPoint);
				}
			}
		}

		protected override void SelectTool(bool state)
		{			
			if (state) 
			{
				Input.action.started += OnInteractTool;
				Input.action.canceled += OnInteractTool;
			}
			else
			{
				Input.action.started -= OnInteractTool;
				Input.action.canceled -= OnInteractTool;
				
				ChangeInputState(state);
			}
			
			base.SelectTool(state);
		}

		private void ChangeInputState(bool state)
		{
			if (state)
			{
				VFX.Play();
				Audio.Play();
				_isWork = true;
			}
			else
			{
				VFX.Stop();
				Audio.Stop();
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
			if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Distance)
			&& hit.collider.TryGetComponent(out SprayPoint sprayPoint))
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(transform.position, hit.point);
				return;
			}
			
			Gizmos.color = Color.red;
			Gizmos.DrawRay(transform.position, transform.forward * Distance);
		}
	}
}