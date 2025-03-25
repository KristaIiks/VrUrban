using System;
using Extensions.Audio;
using SmartConsole;
using UnityEngine;

namespace ToolsSystem
{
	public sealed class Shovel : PhysicTool
	{
		[SerializeField] private Collider Collider;
		[SerializeField] private Transform EffectsTransform;
		[SerializeField] private MeshRenderer HillObject;
		
		public event Action<CrumblyBlock> OnDig;
		
		private float _angle = 0.5f;
		private bool _isFilled;

		private void Awake() => Reset();
		
		private void Update()
		{
			if (_isFilled && CheckRotation(-_angle))
			{
				ActivateHill(false);
			}
		}
		
		protected override void SelectTool(bool state)
		{
			base.SelectTool(state);
			Collider.enabled = state;
		}

		private void DigObject(CrumblyBlock block)
		{
			if (block == null) { return; }
			
			SConsole.Log(LOG_TAG, $"Try dig - {gameObject.name}");
			if(block.Dig(out CrumblyBlockSettings settings))
			{
				Audio.PlayInstanced(settings.DigSound);
			
				ParticleSystem vfx = Instantiate(
					settings.DigEffect,
					EffectsTransform.position, 
					Quaternion.identity, 
					transform
				);
				Destroy(vfx, vfx.totalTime);
				
				HillObject.material = settings.HillMaterial;
				ActivateHill(true);
				
				OnDig?.Invoke(block);
			}
		}

		private void ActivateHill(bool state)
		{
			_isFilled = state;
			HillObject.gameObject.SetActive(_isFilled);
		}

		private void Reset()
		{
			ActivateHill(false);
			Collider.enabled = false;
		}

		private bool CheckRotation(float angle)
		{
			float dotProduct = Vector3.Dot(transform.up, Vector3.up);

			return dotProduct < angle;
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if (_isGrabbed && !_isFilled && CheckRotation(_angle) 
				&& other.TryGetComponent(out CrumblyBlock block))
			{
				DigObject(block);
			}
		}
	}
}