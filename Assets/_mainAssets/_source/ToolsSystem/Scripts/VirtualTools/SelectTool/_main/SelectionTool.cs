using System;
using Extensions.Audio;
using SmartConsole;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ToolsSystem
{
	[RequireComponent(typeof(LineRenderer), typeof(SphereCollider))]
	public abstract class SelectionTool<T> : VirtualTool where T : Selectable
	{
		[Space(25)]
		[SerializeField, Min(0.1f)] private float RayDistance = 10f;
		[SerializeField] private LayerMask RayMask;
		[SerializeField] private bool ForceShowRay;
		
		[Space(25)]
		[SerializeField] private Gradient CanSelectGradient;
		[SerializeField] private Gradient NoAvailableGradient;
		[SerializeField] private Gradient ErrorSelectGradient;
		
		[Space(25)]
		[SerializeField] private InputActionReference SelectBtn;
		
		[Space(25)]
		[SerializeField] private AudioClip InteractionClip;
		[SerializeField] private AudioClip SelectObjectClip;
		[SerializeField] private AudioClip DeSelectObjectClip;
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
		private SphereCollider _collider;
		
		protected override void OnValidate()
		{
			base.OnValidate();
			if (!_lineRenderer)
			{
				_lineRenderer = GetComponent<LineRenderer>();
				_lineRenderer.enabled = false;
			}
			
			if (_collider == null)
			{
				_collider = GetComponent<SphereCollider>();
				_collider.isTrigger = true;
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

		public virtual void InteractObject(T obj, SelectFilter filter)
		{
			if (!IsEnabled || !obj) { return; }
			
			SConsole.Log(LOG_TAG, "Try interact with - " + obj.gameObject.name);
			bool interactionResult = obj.TryInteract(out bool canSelect, filter);
			
			if (canSelect)
				Select(obj, filter);
			if (interactionResult && InteractionClip)
				_audio.PlayRandomized(InteractionClip, PitchRange);
		}
		public void InteractObject(T obj) => Select(obj, SelectFilter.Script);
		
		protected virtual void Select(T obj, SelectFilter filter)
		{	
			if(!IsEnabled && !filter.HasFlag(SelectFilter.Script)) { return; }
			
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
			_selectedObject.Select(filter);
			_lineRenderer.enabled = false;
		
			if (SelectObjectClip)
				_audio.PlayRandomized(SelectObjectClip, PitchRange);
			
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
			
			if (DeSelectObjectClip)
				_audio.PlayRandomized(DeSelectObjectClip, PitchRange);
			
			Deselect();
		}
		
		private void CalculateRay()
		{
			if (!IsEnabled) { return; }
			
			_lineRenderer.SetPosition(0, transform.position);
						
			if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, RayDistance, RayMask))
			{
				Transform obj = GetParentObject(hit.transform);
				T component = obj.GetComponent<T>();
				
				_lineRenderer.SetPosition(1, hit.point);
				_lineRenderer.enabled = ForceShowRay || SelectBtn.action.IsPressed() ||
					(obj != _selectedObject?.transform && (component?.CanSelect ?? false)
				);
				
				if(obj != _lastCheckedObject && obj != _selectedObject?.transform)
				{
					_lastCheckedObject = obj;
					RayObject = component;
				}
			}
			else
			{
				_lineRenderer.SetPosition(1, transform.position + transform.forward * RayDistance);
				_lineRenderer.enabled = ForceShowRay || SelectBtn.action.IsPressed();
				
				RayObject = null;
				_lastCheckedObject = null;
			}
		}
		
		private Transform GetParentObject(Transform obj)
		{
			while (obj.parent != null && !obj.CompareTag(Selectable.OBJECT_TAG)) // Find selectable object
			{
				obj = obj.parent;
			}
			return obj;
		}
		
		private void SetRayColor(Gradient _gradient) => _lineRenderer.colorGradient = _gradient;
		private void SetRayColor(bool state) => SetRayColor(state ? CanSelectGradient : NoAvailableGradient);
		private void InteractObject(InputAction.CallbackContext ctx) => InteractObject(RayObject, SelectFilter.Ray);
		
		private void OnTriggerEnter(Collider other)
		{
			Transform obj = GetParentObject(other.transform);
			
			if (obj.CompareTag(Selectable.OBJECT_TAG) && obj.TryGetComponent(out T component))
			{
				InteractObject(component, SelectFilter.Zone);
			}
		}

		private void OnDrawGizmosSelected()
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