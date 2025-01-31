using System;
using UnityEngine;
using SmartConsole;

namespace ToolsSystem
{
	public sealed class SolidBlock : BaseToolObject
	{
		private const string LOG_TAG = "SolidBlock";
		
		[SerializeField] private GameObject[] _blockStages;
		[SerializeField] private SolidBlockSettings _settings;
		
		public event Action<float> OnDamage;
		public event Action OnDestroy;
		private event Action _studyEvent;
		
		private float _stageHealth { get => _settings.Health / _blockStages.Length; }
		private float _health;
		
		private bool _canInteract;
		
		private void Awake()
		{
			if (!_settings)
			{
				SConsole.LogException(LOG_TAG, new NullReferenceException(), _settings);
				gameObject.SetActive(false);
			}
			else
			{
				Restart(false);
			}
		}
		
		public bool ApplyDamage(float damage, out SolidBlockSettings settings)
		{
			if (!_canInteract) { settings = null; return false; }
			
			SConsole.Log(LOG_TAG, $"Block: {gameObject.name} takes damage \nHP reduced from {_health} to {_health - damage}");
			_health -= damage;
			OnDamage?.Invoke(_health);
			
			if (_health <= 0)
			{
				Destroy();
				settings = _settings;
				return true;
			}
			SetModel();
			
			settings = null;
			return false;
		}
		
		private void SetModel()
		{
			int index = _blockStages.Length - Mathf.CeilToInt(_health / _stageHealth);
			for (int i = 0; i < _blockStages.Length; i++)
			{
				if (i == index) { _blockStages[i].SetActive(true); continue; }
				_blockStages[i].SetActive(false);
			}
		}
		
		private void Destroy()
		{
			gameObject.SetActive(false);
			SConsole.Log(LOG_TAG, $"Destroy - {gameObject.name}");
			OnDestroy?.Invoke();
		}

		public override void StartDefaultStudy(Action OnComplete = null)
		{
			Restart(true);
			
			_studyEvent = () => { OnComplete.Invoke(); OnDestroy -= _studyEvent; };
			OnDestroy += _studyEvent;
		}

		public override void Restart(bool canContinue = true)
		{
			_canInteract = canContinue;
			
			gameObject.SetActive(true);
			_health = _settings.Health;
			SetModel();
		}
	}
}