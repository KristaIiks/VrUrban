using System;
using UnityEngine;

namespace ToolsSystem
{
	[RequireComponent(typeof(Collider))]
	public sealed class CrumblyBlock : BaseToolObject
	{
		public const string BLOCK_TAG = "CrumblyBlock";
		
		[SerializeField] private CrumblyBlockSettings _blockSettings;
		
		public event Action OnDig;
		
		private bool _canDig;
		
		private void OnValidate()
		{
			gameObject.tag = BLOCK_TAG;
		}
		
		public bool Dig(out CrumblyBlockSettings settings)
		{
			if (!_canDig) { settings = null; return false; }
			
			gameObject.SetActive(false);
			OnDig?.Invoke(); 
			
			settings = _blockSettings;
			return true;
		}
	}
}
