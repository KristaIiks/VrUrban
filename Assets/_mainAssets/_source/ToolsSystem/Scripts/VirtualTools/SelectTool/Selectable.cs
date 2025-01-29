using System;
using UnityEngine;
using QuickOutline;

namespace ToolsSystem
{
	[RequireComponent(typeof(Outline), typeof(Collider))]
	public abstract class Selectable : BaseToolObject
	{		
		public const string OBJECT_TAG = "Selectable";
		
		[SerializeField] private VisualMode VisualMode;
		[SerializeField] private SelectFilter SelectFilter;
		
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
				_outline.enabled = false;
			}
			gameObject.tag = OBJECT_TAG;
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
		
		protected bool CompareFilter(SelectFilter filter)
		{
			if (SelectFilter == SelectFilter.All) { return true; } // Can interact with any types
			
			return filter == SelectFilter;
		}
	}
}
