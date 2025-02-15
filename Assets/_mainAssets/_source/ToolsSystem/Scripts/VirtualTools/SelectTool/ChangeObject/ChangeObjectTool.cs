using System;
using Extensions.Audio;
using UnityEngine;

namespace ToolsSystem
{
	public sealed class ChangeObjectTool : SelectionTool<Changeable>
	{		
		// TODO: remove and replace by zenject
		[Space(25)]
		[SerializeField] private ChangeWindow Menu;
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
			if (!_selectedObject) { return; }
			
			_selectedObject.ChangeBuild(id);
			
			_audio.PlayRandomized(ChangeObjectClip, PitchRange);
			Deselect();
			
			OnObjectChanged?.Invoke(_selectedObject, id);
		}

		private void OpenMenu(SelectFilter filter)
		{
			if (filter == SelectFilter.Zone)
			{
				Menu.Open(_selectedObject.Variants, _selectedObject.UIPosition.position, this);
				return;
			}
			Menu.Open(_selectedObject.Variants, Vector3.zero, this);			
		}
		private void CloseMenu() => Menu.Close();
	}
}
