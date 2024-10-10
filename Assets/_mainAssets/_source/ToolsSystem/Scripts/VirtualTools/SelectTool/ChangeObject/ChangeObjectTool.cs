using System;
using Extensions.Audio;
using UnityEngine;

namespace ToolsSystem
{
	public sealed class ChangeObjectTool : SelectTool<Changeable>
	{		
		[SerializeField] private ChangeWindow Menu;
		[SerializeField] private AudioClip ChangeObjectClip;
		[SerializeField] private Vector2 PitchRange;
		
		public event Action<Changeable, int> OnObjectChanged;

		public override void Select(Changeable obj)
		{
			base.Select(obj);
			OpenMenu();
		}

		public override void Deselect()
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

		private void OpenMenu() => Menu.Open(_selectedObject.Variants, this);
		private void CloseMenu() => Menu.Close();
	}
}
