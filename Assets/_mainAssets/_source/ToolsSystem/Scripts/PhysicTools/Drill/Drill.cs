using System;
using System.Collections.Generic;
using UnityEngine;
using Extensions.Audio;

namespace ToolsSystem
{
	public sealed class Drill : PhysicTool
	{
		[SerializeField] private Collider _trigger;
		[SerializeField] private GameObject _vfxObject;
		[SerializeField] private float _drillSpeed;
		[SerializeField] private Vector2 _pitchRange = new Vector2(-1f, 1f);
		
		public event Action Performed;
		public event Action<SolidBlock> OnDrillObject;
		
		private List<SolidBlock> _blocks = new List<SolidBlock>();

		private void Awake() => Reset();
		
		private void Update()
		{
			if (!_isGrabbed && _blocks.Count != 0) { return; }
			
			int length = _blocks.Count - 1;
			for (int i = length; i >= 0; i--)
			{
				if (_blocks[i].ApplyDamage(_drillSpeed * Time.deltaTime, out SolidBlockSettings settings))
				{					
					_audio.PlayRandomized(settings.BreakSound, _pitchRange);
					
					OnDrillObject?.Invoke(_blocks[i]);
					_blocks.RemoveAt(i);
				}					
			}
			Performed?.Invoke();
		}
		
		protected override void SelectTool(bool state)
		{
			base.SelectTool(state);
			_trigger.isTrigger = state;
		}

		private  void Reset()
		{
			_trigger.isTrigger = false;
			_vfxObject.SetActive(false);
		}

		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent(out SolidBlock block))
			{
				if (_blocks.Count == 0)
				{
					_vfxObject.SetActive(true);
					_audio.Play();
				}
					
				_blocks.Add(block);
			}
		}
		
		private void OnTriggerExit(Collider other)
		{
			if(other.TryGetComponent(out SolidBlock block))
			{
				_blocks.Remove(block);
				
				if (_blocks.Count == 0)
				{
					_vfxObject.SetActive(false);
					_audio.Stop();
				}
			}
		}
	}
}
