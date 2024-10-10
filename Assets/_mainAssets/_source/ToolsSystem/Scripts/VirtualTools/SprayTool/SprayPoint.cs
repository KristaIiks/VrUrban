using System;
using UnityEngine;

namespace ToolsSystem
{
	[RequireComponent(typeof(Collider))]
	public abstract class SprayPoint : BaseToolObject
	{
		public const string GRASS_TAG = "SprayPoint";
		
		[SerializeField] private GameObject _grassObject;
		
		public abstract event Action Painted;
		
		private Collider _collider;

		private void OnValidate()
		{
			_collider ??= GetComponent<Collider>();
		}

		public abstract void Paint();
	}
}
