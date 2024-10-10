using System;
using UnityEngine;

namespace ToolsSystem
{
	public sealed class GrassPoint : SprayPoint
	{
		[SerializeField] private Material _grassMaterial;

		public override event Action Painted;

		private MeshRenderer _meshRenderer;
		private bool _canPaint;
		
		public override void Paint()
		{
			if (!_canPaint) { return; }
			
			_meshRenderer.material = _grassMaterial;
			_canPaint = false;
			Painted?.Invoke();
		}
	}
}
