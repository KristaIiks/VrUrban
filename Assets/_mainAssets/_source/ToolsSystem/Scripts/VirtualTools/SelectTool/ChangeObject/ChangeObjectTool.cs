using System;
using Extensions.Audio;
using UnityEngine;

namespace ToolsSystem
{
	public sealed class ChangeObjectTool : SelectionTool<Changeable>
	{		
		[Space(25)]
		// TODO: remove and replace by zenject
		[SerializeField] private ChangeWindow Menu;
		[SerializeField] private Transform RayMenuPos;
		[SerializeField] private AudioClip ChangeObjectClip;
		
		public event Action<Changeable, int> OnObjectChanged;

		protected override void Select(Changeable obj, SelectFilter filter)
		{
			base.Select(obj, filter);
			OpenMenu(filter);
		}

		protected override void Deselect()
		{
			base.Deselect();
			CloseMenu();
		}

		public void SelectVariant(int id)
		{
			if (!_selectedObject)
				return;
			
			Audio.PlayRandomized(ChangeObjectClip, PitchRange);
			
			Changeable obj = _selectedObject;
			Deselect();
			
			// TODO: Bad sorting for study need rework
			obj.ChangeBuild(id);
			OnObjectChanged?.Invoke(obj, id);
		}

		private void OpenMenu(SelectFilter filter)
		{
			if (filter == SelectFilter.Zone)
			{
				Menu.Open(_selectedObject.Variants, _selectedObject.UIPosition, this);
				return;
			}
			
			Menu.Open(_selectedObject.Variants, RayMenuPos, this);			
		}
		private void CloseMenu() => Menu.Close();
	}
}