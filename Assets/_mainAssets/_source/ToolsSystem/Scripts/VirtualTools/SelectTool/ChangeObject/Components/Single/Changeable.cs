using System;
using System.Collections.Generic;
using SmartConsole;
using UnityEngine;

namespace ToolsSystem
{
	public class Changeable : Selectable
	{
		private const string LOG_TAG = "Changeable";
		
		[field:Space(25)]
		[field:SerializeField] public List<ChangeVariant> Variants { get; protected set; }
		[SerializeField] private GameObject _defaultObject;
		[field:SerializeField] public Transform UIPosition { get; private set; }
		
		public event Action<int> OnObjectChanged;
		private event Action<int> _studyEvent;
		
		protected virtual void Awake() => Restart(false);
		
		public virtual void ChangeBuild(int id)
		{
			id = Mathf.Clamp(id, 0, Variants.Count - 1);
			
			if (_defaultObject) { _defaultObject.SetActive(false); }
			
			for (int i = 0; i < Variants.Count - 1; i++)
			{
				Variants[i].Object?.SetActive(false);
				Variants[i].IsSelected = false;
			}
			
			if (Variants[id].Object != null) { Variants[id].Object.SetActive(true); }
			Variants[id].IsSelected = true;
			
			if (Variants[id].Object != null)
				SConsole.Log(LOG_TAG, $"Change [{gameObject.name}] object to [{Variants[id].Object.name}][{id}]");
			else
				SConsole.Log(LOG_TAG, $"Change [{gameObject.name}] object to [Nothing][{id}]");
			
			OnObjectChanged?.Invoke(id);
		}
		
		public void HideVariant(int id) => HideVariant(id, true);
		public void HideVariant(int id, bool value)
		{
			if (id >= Variants.Count) { return; }
			
			Variants[id].IsHidden = value;
		}
		
		public override void StartDefaultStudy(Action OnComplete = null)
		{
			CanSelect = true;
			
			_studyEvent = (id) => { OnComplete?.Invoke(); OnObjectChanged -= _studyEvent; };
			OnObjectChanged += _studyEvent;
		}

		public override void Skip()
		{
			Restart(false);
			ChangeBuild(UnityEngine.Random.Range(0, Variants.Count - 1));
		}

		public override void Restart(bool canContinue)
		{
			if (_defaultObject) { _defaultObject.SetActive(true); }
			
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
				variant.IsHidden = false;
			}
			
			CanSelect = canContinue;
			SConsole.Log(LOG_TAG, $"Reset {gameObject.name}", 2);
		}
	}
}