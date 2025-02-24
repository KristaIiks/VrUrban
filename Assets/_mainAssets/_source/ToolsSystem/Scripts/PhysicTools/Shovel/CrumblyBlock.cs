using System;
using QuickOutline;
using SmartConsole;
using UnityEngine;

namespace ToolsSystem
{
	[RequireComponent(typeof(Collider), typeof(Outline))]
	public sealed class CrumblyBlock : BaseToolObject
	{
		[SerializeField] private CrumblyBlockSettings _blockSettings;
		[SerializeField] private Outline Outline;
		
		public event Action OnDig;
		private event Action _studyEvent;
		
		private bool _canDig;
		
		private void OnValidate()
		{
			if(!Outline)
			{
				Outline = GetComponent<Outline>();
				Outline.OutlineWidth = 8f;
				Outline.OutlineColor = Color.yellow;
				
				Outline.enabled = false;				
			}
		}
		
		public bool Dig(out CrumblyBlockSettings settings)
		{
			SConsole.Log("Tool: Shovel", $"Try dig - {gameObject.name}");
			if (!_canDig) { settings = null; return false; }
			
			gameObject.SetActive(false);
			_canDig = false;
			
			OnDig?.Invoke(); 
			
			SConsole.Log("Tool: Shovel", $"Successful dig");
			settings = _blockSettings;
			return true;
		}

		public override void StartDefaultStudy(Action OnComplete = null)
		{
			Restart(true);
			
			_studyEvent = () => { OnComplete?.Invoke(); OnDig -= _studyEvent; };
			OnDig += _studyEvent;
		}

		public override void Skip()
		{
			Restart();
			Dig(out _);
		}

		public override void Restart(bool canContinue = true)
		{
			_canDig = canContinue;
			Outline.enabled = canContinue;
			gameObject.SetActive(true);
		}
	}
}
