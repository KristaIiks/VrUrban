using System;
using QuickOutline;
using SmartConsole;
using UnityEngine;

namespace ToolsSystem
{
	[RequireComponent(typeof(Outline))]
	public abstract class Selectable : BaseToolObject
	{		
		public const string OBJECT_TAG = "Selectable";
		
		[SerializeField] private VisualMode VisualMode = VisualMode.Outline;
		[SerializeField] private SelectFilter SelectFilter = SelectFilter.Ray;
		
		public bool ObjectState { get => CanInteract || CanSelect; }
		public bool CanInteract 
		{
			get => _canInteract;
			protected set
			{
				bool lastState = ObjectState;
				
				if (lastState != ObjectState)
					OnStateChanged?.Invoke(ObjectState);
				
				_canInteract = value;
			}
		}
		public bool CanSelect
		{
			get => _canSelect;
			protected set
			{
				bool lastState = ObjectState;
				
				if (lastState != ObjectState)
					OnStateChanged?.Invoke(ObjectState);
				
				_canSelect = value;
			}
		}
		
		public event Action OnInteract;
		public event Action<bool> OnSelectChanged;
		public event Action<bool> OnStateChanged;
		
		protected Outline _outline;
		protected bool _isSelected;
		
		private bool _canInteract;
		private bool _canSelect = true;
		
		protected virtual void OnValidate()
		{
			if(!_outline)
			{
				_outline = GetComponent<Outline>();
				_outline.OutlineWidth = 8f;
				_outline.OutlineColor = Color.yellow;
				_outline.BakeOutline = true;
				
				_outline.enabled = false;
				
				 if (!gameObject.CompareTag(OBJECT_TAG)) { gameObject.tag = OBJECT_TAG; }
			}
			
			if (transform.GetComponentsInChildren<Collider>().Length == 0) { SConsole.Log("Selectable", "Object doesn't have colliders."); }
		}
		
		public virtual bool TryInteract(out bool canSelect, SelectFilter filter)
		{
			if (CanInteract) { OnInteract?.Invoke(); }
			canSelect = CanSelect && !_isSelected && CompareFilter(filter);
			
			return CanInteract;
		}
		
		public virtual void Select(SelectFilter filter)
		{
			if (!CanSelect || _isSelected || !CompareFilter(filter)) { return; }
			
			
			_isSelected = true;
			
			_outline.enabled = VisualMode == VisualMode.Outline;
			// TODO: add zone
			
			OnSelectChanged?.Invoke(true);
		}
		
		public virtual void Deselect()
		{
			if (!_isSelected) { return; }
			
			
			_isSelected = false;
			_outline.enabled = false;
			OnSelectChanged?.Invoke(false);
		}
		
		protected bool CompareFilter(SelectFilter filter) => SelectFilter.HasFlag(filter);
	}
}
