using System;
using UnityEngine;

namespace ToolsSystem
{
	[RequireComponent(typeof(Collider))]
	public sealed class CrumblyBlock : BaseToolObject
	{
		[SerializeField] private CrumblyBlockSettings _blockSettings;
		
		public event Action OnDig;
		
		private bool _canDig;
		
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
