using System;
using SmartConsole;
using UnityEngine;
using UnityEngine.InputSystem;
using Extensions.Audio;

namespace ToolsSystem
{
	[RequireComponent(typeof(LineRenderer))]
	public abstract class SelectionTool<T> : VirtualTool where T : Selectable
	{
		[SerializeField, Min(0.1f)] private float RayDistance = 10f;
		[SerializeField] private LayerMask RayMask;
		[SerializeField] private bool ForceShowRay;
		
		[SerializeField] private Gradient CanSelectGradient;
		[SerializeField] private Gradient NoAvailableGradient;
		[SerializeField] private Gradient ErrorSelectGradient;
		
		[SerializeField] private InputActionReference SelectBtn;
		[SerializeField] private AudioClip InteractionClip;
		[SerializeField] private AudioClip SelectClip;
		[SerializeField] private AudioClip DeSelectClip;
		[SerializeField] protected Vector2 PitchRange = new Vector2(-0.1f, .1f);
		
		public event Action<T> OnSelect;
		public event Action<T> OnDeselect;
		public event Action<T> OnHover;
		
		protected LineRenderer _lineRenderer;
		protected T _selectedObject;		

		private T RayObject
		{
			get => _rayObject;
			set 
			{
				if (_rayObject) { _rayObject.OnStateChanged -= SetRayColor; }
				
				_rayObject = value;
				
				if (_rayObject) 
				{
					_rayObject.OnStateChanged += SetRayColor;
					SetRayColor(_rayObject.ObjectState);
				}
				else
				{
					SetRayColor(ErrorSelectGradient);
				}
				OnHover?.Invoke(_rayObject);
			}
		}
		private T _rayObject;
		private Transform _lastCheckedObject;
		
		protected override void OnValidate()
		{
			base.OnValidate();
			if (!_lineRenderer)
			{
				_lineRenderer = GetComponent<LineRenderer>();
				_lineRenderer.enabled = false;
			}
		}
		
		protected virtual void OnEnable()
		{
			SelectBtn.action.performed += InteractObject;
		}
		
		protected virtual void OnDisable()
		{
			SelectBtn.action.performed -= InteractObject;
		}
		
		protected virtual void Update() => CalculateRay();

		protected override void SelectTool(bool state)
		{
			if(!state)
			{
				DeselectWithSound();
			}
			base.SelectTool(state);
		}

		public virtual void InteractObject(T obj)
		{
			if (!IsEnabled || !obj) { return; }
			
			SConsole.Log(LOG_TAG, "Try interact with - " + obj.gameObject.name);
			bool interactionResult = obj.TryInteract(out bool canSelect);
			
			if (canSelect)
				Select(obj);
			if (interactionResult)
				_audio.PlayRandomized(InteractionClip, PitchRange);
		}
		
		public virtual void Select(T obj)
		{	
			if(!IsEnabled) { return; }
			
			if(_selectedObject || obj == null)
			{
				if(obj == null)
				{
					DeselectWithSound();
					return;
				}
				else
				{
					Deselect();
				}
			}
			
			_selectedObject = obj;
			_selectedObject.Select();
		
			_audio.PlayRandomized(SelectClip, PitchRange);
			SConsole.Log(LOG_TAG, "Select object - " + obj.gameObject.name);
			
			OnSelect?.Invoke(obj);
		}
		
		protected virtual void Deselect()
		{			
			_selectedObject?.Deselect();			
			OnDeselect?.Invoke(_selectedObject);
			_selectedObject = null;
			
			SConsole.Log(LOG_TAG, "Deselect object");
		}
		
		protected void DeselectWithSound()
		{
			if (!_selectedObject) { return; }
			
			_audio.PlayRandomized(DeSelectClip, PitchRange);
			Deselect();
		}
		
		private void CalculateRay()
		{
			if (!IsEnabled) { return; }
			
			_lineRenderer.SetPosition(0, transform.position);
						
			if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, RayDistance, RayMask))
			{
				Transform obj = hit.transform;

				while (obj.parent != null && !obj.CompareTag(Selectable.OBJECT_TAG)) // Find selectable object
				{
					obj = obj.parent;
				}
				
				if (!obj.CompareTag(Selectable.OBJECT_TAG)) { return; }
				
				if(obj == RayObject?.transform) // Object same (update position)
				{
					SetRayPosition(1, hit.point);
					return;
				}
				
				if(obj != _lastCheckedObject) // New object (try)
				{
					_lastCheckedObject = obj;
					if (obj.TryGetComponent(out T component))
					{
						SetRayPosition(1, hit.point);
						RayObject = component;
						return;
					}
				}
				
				// If error
			}
			
			if(ForceShowRay || SelectBtn.action.IsPressed())
			{
				SetRayPosition(1, transform.position + transform.forward * RayDistance);
			}
			else
			{
				_lineRenderer.enabled = false;
			}
			
			_lastCheckedObject = null;
			RayObject = null;
		}
		
		private void SetRayColor(Gradient _gradient) => _lineRenderer.colorGradient = _gradient;
		private void SetRayColor(bool state) => SetRayColor(state ? CanSelectGradient : NoAvailableGradient);
		private void SetRayPosition(int id, Vector3 pos)
		{
			_lineRenderer.SetPosition(id, pos);
			_lineRenderer.enabled = true;
		}
		private void InteractObject(InputAction.CallbackContext ctx) => InteractObject(RayObject);
		
		private void OnDrawGizmos()
		{
			if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, RayDistance, RayMask) && RayObject) 
			{
				Gizmos.color = RayObject.CanSelect ? Color.green : Color.yellow;
				Gizmos.DrawLine(transform.position, hit.point);
				return;
			}
			
			Gizmos.color = Color.red;
			Gizmos.DrawRay(transform.position, transform.forward * RayDistance);
		}
	}
}