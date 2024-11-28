using System;
using UnityEngine;
using SmartConsole;

namespace ToolsSystem
{
	public sealed class Changeable : Selectable
	{
		private const string LOG_TAG = "Changeable";
		
		[field:SerializeField] public ChangeVariant[] Variants { get; private set; }
		[SerializeField] private GameObject _defaultObject;
		
		public event Action<int> OnObjectChanged;
		
		private void Awake() => Reset();
		public void ChangeBuild(int id)
		{
			if (!_isSelected) { return; }
			
			id = Mathf.Clamp(id, 0, Variants.Length);
			
			_defaultObject.SetActive(false);
			for (int i = 0; i < Variants.Length; i++)
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
			
			for(int i = 0; i < Variants.Length; i++)
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
	}
}
