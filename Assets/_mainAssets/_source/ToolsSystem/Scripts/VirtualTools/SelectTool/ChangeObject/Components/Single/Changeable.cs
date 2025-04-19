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
		[SerializeField] private GameObject DefaultObject;
		
		//? Other world ui system?
		[field:SerializeField] public Transform UIPosition { get; private set; }
		
		public event Action<int> OnObjectChanged;
		
		private event Action<int> _studyEvent;
		
		protected virtual void Awake() => Restart(false);
		
		public virtual void ChangeBuild(int id)
		{
			id = Mathf.Clamp(id, 0, Variants.Count - 1);
			
			if (DefaultObject)
				DefaultObject.SetActive(false);
			
			for (int i = 0; i < Variants.Count; i++)
			{
				if (Variants[i].Object)
					Variants[i].Object.SetActive(false);
				
				Variants[i].IsSelected = false;
			}
			
			if (Variants[id].Object)
				Variants[id].Object.SetActive(true);
			
			Variants[id].IsSelected = true;
			
			if (Variants[id].Object)
				SConsole.Log(LOG_TAG, $"Change [{gameObject.name}] object to [{Variants[id].Object.name}][{id}]");
			else
				SConsole.Log(LOG_TAG, $"Change [{gameObject.name}] object to [Nothing][{id}]");
			
			OnObjectChanged?.Invoke(id);
		}
		
		public void HideVariant(int id, bool value)
		{
			if (id >= Variants.Count)
				return;
			
			Variants[id].IsHidden = value;
		}
		public void HideVariant(int id) => HideVariant(id, true);
		
		public override void StartDefaultStudy(Action OnComplete = null)
		{
			base.StartDefaultStudy(OnComplete);
			
			CanSelect = true;
			
			_studyEvent = (id) => { OnComplete?.Invoke(); CanSelect = false; OnObjectChanged -= _studyEvent; };
			OnObjectChanged += _studyEvent;
		}

		public override void Skip()
		{
			Restart(false);
			ChangeBuild(UnityEngine.Random.Range(0, Variants.Count - 1));
		}

		public override void Restart(bool canContinue)
		{
			if (DefaultObject) { DefaultObject.SetActive(true); }
			
			for(int i = 0; i < Variants.Count - 1; i++)
			{
				ChangeVariant variant = Variants[i];
				
				if (DefaultObject != variant.Object)
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
			Deselect();
			
			SConsole.Log(LOG_TAG, $"Reset {gameObject.name}", 2);
		}
	}
}