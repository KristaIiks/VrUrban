using System;
using UnityEngine;

namespace ToolsSystem
{
	[RequireComponent(typeof(MeshRenderer))]
	public sealed class GrassPoint : SprayPoint
	{
		[SerializeField] private Material _grassMaterial;

		public override event Action OnPaint;

		private MeshRenderer _meshRenderer;
		private bool _canPaint;
		
		private void OnValidate()
		{
			_meshRenderer ??= GetComponent<MeshRenderer>();
		}
		
		public override void Paint()
		{
			if (!_canPaint) { return; }
			
			_meshRenderer.material = _grassMaterial;
			_canPaint = false;
			OnPaint?.Invoke();
		}
	}
}
