using System;
using UnityEngine;
using QuickOutline;

namespace ToolsSystem
{
	[RequireComponent(typeof(Outline))]
	public abstract class Selectable : BaseToolObject
	{
		public const string OBJECT_TAG = "SelectObject";
		
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
			gameObject.tag = OBJECT_TAG;
			
			if(!_outline)
			{
				_outline = GetComponent<Outline>();
				_outline.OutlineWidth = 8f;
			}
		}
		
		public virtual bool TryInteract(out bool canSelect)
		{
			if (CanInteract) { OnInteract?.Invoke(); }
			canSelect = CanSelect;
			
			return CanInteract;
		}
		
		public virtual void Select()
		{
			if (!CanSelect || _isSelected) { return; }
			
			
			_isSelected = true;
			_outline.enabled = _isSelected;
			OnSelectChanged?.Invoke(_isSelected);
		}
		
		public virtual void Deselect()
		{
			if (!_isSelected) { return; }
			
			
			_isSelected = false;
			_outline.enabled = _isSelected;
			OnSelectChanged?.Invoke(_isSelected);
		}
	}
}
