using System;
using UnityEngine;
using SmartConsole;
using System.Collections.Generic;

namespace ToolsSystem
{
	public class Changeable : Selectable
	{
		private const string LOG_TAG = "Changeable";
		
		[field:SerializeField] public List<ChangeVariant> Variants { get; protected set; }
		[field:SerializeField] public Transform UIPosition { get; private set; }
		[SerializeField] private GameObject _defaultObject;
		
		public event Action<int> OnObjectChanged;
		
		protected virtual void Awake() => Restart(false);
		
		public virtual void ChangeBuild(int id)
		{
			if (!_isSelected) { return; }
			
			id = Mathf.Clamp(id, 0, Variants.Count - 1);
			
			_defaultObject.SetActive(false);
			for (int i = 0; i < Variants.Count - 1; i++)
			{
				Variants[i].Object.SetActive(false);
				Variants[i].IsSelected = false;
			}
			
			Variants[id].Object.SetActive(true);
			Variants[id].IsSelected = true;
			
			SConsole.Log(LOG_TAG, $"Change [{gameObject.name}] object to [{Variants[id].Object.name}][{id}]");
			OnObjectChanged?.Invoke(id);
		}
		
		private void Reset()
		{
			_defaultObject.SetActive(true);
			
			for(int i = 0; i < Variants.Count - 1; i++)
			{
				ChangeVariant variant = Variants[i];
				
				if (_defaultObject != variant.Object)
				{
					variant.Object.SetActive(false);
					variant.IsSelected = false;
				}
				else
				{
					variant.IsSelected = true;
				}				
			}
			
			SConsole.Log(LOG_TAG, $"Reset {gameObject.name}");
		}
		
		public override void StartDefaultStudy(Action OnComplete = null)
		{
			Restart(true);
			//
		}

		public override void Restart(bool canContinue = true)
		{
			Reset();
			CanSelect = canContinue;
		}
	}
}
