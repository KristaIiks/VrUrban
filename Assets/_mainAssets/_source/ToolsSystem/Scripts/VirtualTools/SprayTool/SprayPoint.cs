using System;
using UnityEngine;

namespace ToolsSystem
{
	[RequireComponent(typeof(Collider))]
	public abstract class SprayPoint : BaseToolObject
	{
		public const string SPRAY_POINT_TAG = "SprayPoint";
		
		public abstract event Action OnPaint;
		
		private Collider _collider;

		private void OnValidate()
		{
			gameObject.tag = SPRAY_POINT_TAG;
			_collider ??= GetComponent<Collider>();
		}

		public abstract void Paint();
	}
}
