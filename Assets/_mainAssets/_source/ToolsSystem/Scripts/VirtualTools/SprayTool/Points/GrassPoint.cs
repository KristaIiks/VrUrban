using System;
using UnityEngine;

namespace ToolsSystem
{
	[RequireComponent(typeof(MeshRenderer))]
	public sealed class GrassPoint : SprayPoint
	{
		[SerializeField] private Material GroundMaterial;
		[SerializeField] private Material GrassMaterial;

		public override event Action OnPaint;
		private event Action _studyEvent;

		private MeshRenderer _meshRenderer;
		private bool _canPaint;
		
		private void OnValidate()
		{
			_meshRenderer ??= GetComponent<MeshRenderer>();
		}
		
		public override bool Paint()
		{
			if (!_canPaint) { return false; }
			
			_meshRenderer.material = GrassMaterial;
			_canPaint = false;
			OnPaint?.Invoke();
			
			return true;
		}

		public override void StartDefaultStudy(Action OnComplete = null)
		{
			Restart(true);
			
			_studyEvent = () => { OnComplete.Invoke(); OnPaint -= _studyEvent; };
			OnPaint += _studyEvent;
		}

		public override void Skip()
		{
			Restart();
			Paint();
		}

		public override void Restart(bool canContinue = true)
		{			
			_canPaint = canContinue;
			_meshRenderer.material = GroundMaterial;
		}
	}
}
