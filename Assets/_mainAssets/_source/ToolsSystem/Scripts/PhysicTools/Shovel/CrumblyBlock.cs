using System;
using UnityEngine;

namespace ToolsSystem
{
	[RequireComponent(typeof(Collider))]
	public sealed class CrumblyBlock : BaseToolObject
	{
		[SerializeField] private CrumblyBlockSettings _blockSettings;
		
		public event Action OnDig;
		private event Action _studyEvent;
		
		private bool _canDig;
		
		public bool Dig(out CrumblyBlockSettings settings)
		{
			if (!_canDig) { settings = null; return false; }
			
			gameObject.SetActive(false);
			OnDig?.Invoke(); 
			
			settings = _blockSettings;
			return true;
		}

		public override void StartDefaultStudy(Action OnComplete = null)
		{
			Restart(true);
			
			_studyEvent = () => { OnComplete?.Invoke(); OnDig -= _studyEvent; };
			OnDig += _studyEvent;
		}

		public override void Restart(bool canContinue = true)
		{
			_canDig = canContinue;
			gameObject.SetActive(true);
		}
	}
}
