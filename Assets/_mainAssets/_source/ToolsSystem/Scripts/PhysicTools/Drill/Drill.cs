using System;
using System.Collections.Generic;
using Extensions.Audio;
using UnityEngine;

namespace ToolsSystem
{
	public sealed class Drill : PhysicTool
	{
		[SerializeField] private Collider Trigger;
		[SerializeField] private GameObject VfxObject;
		[SerializeField] private float DrillSpeed;
		
		public event Action<SolidBlock> OnDrillObject;
		public event Action Performed;
		
		private List<SolidBlock> _blocks = new List<SolidBlock>();

		private void Awake() => Reset();
		
		private void Update()
		{
			if (!_isGrabbed && _blocks.Count == 0) { return; }
			
			for (int i = _blocks.Count - 1; i >= 0; i--)
			{
				if (_blocks[i].ApplyDamage(DrillSpeed * Time.deltaTime, out SolidBlockSettings settings))
				{
					if (settings.BreakSound)
						Audio.PlayInstanced(settings.BreakSound);
					
					OnDrillObject?.Invoke(_blocks[i]);
					_blocks.RemoveAt(i);
					
					UpdateVisuals();
				}					
			}
			Performed?.Invoke();
		}
		
		protected override void SelectTool(bool state)
		{
			base.SelectTool(state);
			Trigger.isTrigger = state;
		}

		private void Reset()
		{
			Trigger.isTrigger = false;
			VfxObject.SetActive(false);
		}

		private void UpdateVisuals()
		{
			if (_blocks.Count == 0)
			{
				VfxObject.SetActive(false);
				Audio.Stop();
				return;
			}
			
			if (!Audio.isPlaying)
			{
				VfxObject.SetActive(true);
				Audio.Play();
			}
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent(out SolidBlock block))
			{
				_blocks.Add(block);
				UpdateVisuals();
			}
		}
		
		private void OnTriggerExit(Collider other)
		{
			if(other.TryGetComponent(out SolidBlock block))
			{
				_blocks.Remove(block);
				UpdateVisuals();
			}
		}
	}
}