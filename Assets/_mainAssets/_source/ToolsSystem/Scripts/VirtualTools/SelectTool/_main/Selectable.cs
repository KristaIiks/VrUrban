using System;
using QuickOutline;
using SmartConsole;
using UnityEngine;

namespace ToolsSystem
{
	public abstract class Selectable : BaseToolObject
	{		
		public const string OBJECT_TAG = "Selectable";
		
		[SerializeField] private SelectFilter SelectFilter = SelectFilter.Ray;
		[SerializeField] protected Outline SelectOutline;
		
		public bool ObjectState { get => CanInteract || CanSelect; }
		public bool CanInteract 
		{
			get => m_canInteract;
			protected set
			{
				bool lastState = ObjectState;
				m_canInteract = value;
				
				if (lastState != ObjectState)
					OnStateChanged?.Invoke(ObjectState);
			}
		}
		public bool CanSelect
		{
			get => m_canSelect;
			set
			{
				bool lastState = ObjectState;
				m_canSelect = value;
				
				if (lastState != ObjectState)
					OnStateChanged?.Invoke(ObjectState);
			}
		}
		
		public event Action OnInteract;
		public event Action<bool> OnStateChanged;
		public event Action<bool> OnSelectChanged;
		
		protected bool _isSelected;
		
		private bool m_canInteract;
		private bool m_canSelect = true;
		
		protected virtual void OnValidate()
		{
			if(!SelectOutline && TryGetComponent(out SelectOutline))
			{
				SelectOutline.OutlineWidth = 8f;
				SelectOutline.OutlineColor = Color.yellow;
				SelectOutline.BakeOutline = true;
				
				SelectOutline.enabled = false;				
			}
			
			if (!gameObject.CompareTag(OBJECT_TAG))
				 gameObject.tag = OBJECT_TAG;
				 
			if (transform.GetComponentsInChildren<Collider>(true).Length == 0)
				SConsole.Log("Selectable", "Object doesn't have colliders.");
		}
		
		// TODO: add interact void
		//? remove canInteract and create base interaction(+select) logic splitted to protected voids 
		public virtual bool TryInteract(out bool canSelect, SelectFilter filter)
		{
			if (CanInteract)
				OnInteract?.Invoke(); // replace and move to void
			
			canSelect = CanSelect && !_isSelected && CompareFilter(filter);
			
			return CanInteract;
		}
		
		public virtual void Select(SelectFilter filter)
		{
			if (!CompareFilter(SelectFilter.Script) && (!CanSelect || _isSelected || !CompareFilter(filter)))
				return;
			
			_isSelected = true;
			
			if (SelectOutline)
				SelectOutline.enabled = true;
			
			OnSelectChanged?.Invoke(true);
		}
		
		public virtual void Deselect()
		{
			if (!_isSelected)
				return;
			
			_isSelected = false;
			SelectOutline.enabled = false;
			
			OnSelectChanged?.Invoke(false);
		}
		
		protected bool CompareFilter(SelectFilter filter) => SelectFilter.HasFlag(filter);
	}
}