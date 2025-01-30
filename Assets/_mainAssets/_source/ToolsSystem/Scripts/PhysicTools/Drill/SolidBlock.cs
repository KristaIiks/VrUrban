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
		
		private float _stageHealth { get => _settings.Health / _blockStages.Length; }
		private float _health;
		
		private void Awake()
		{
			if (!_settings)
			{
				SConsole.LogException(LOG_TAG, new NullReferenceException(), _settings);
				gameObject.SetActive(false);
			}
			else
			{
				_health = _settings.Health;
				SetModel();
			}
		}
		
		public bool ApplyDamage(float damage, out SolidBlockSettings settings)
		{
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
	}
}