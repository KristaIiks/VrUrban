using System;
using UnityEngine;
using SmartConsole;

namespace ToolsSystem
{
	public sealed class SolidBlock : BaseToolObject
	{
		private const string LOG_TAG = "SolidBlock";
		
		[SerializeField] private float _health;
		[SerializeField] private GameObject[] _blockStages;
		
		public event Action<float> OnDamage;
		public event Action OnDestroy;
		public SolidBlockSettings Settings;
		
		private float _stageHealth { get => Settings.Health / _blockStages.Length; }
		
		private void Awake()
		{
			if (!Settings)
			{
				SConsole.LogException(LOG_TAG, new NullReferenceException(), Settings);
				gameObject.SetActive(false);
			}
			else
			{
				_health = Settings.Health;
				SetModel();
			}
		}
		
		public bool ApplyDamage(float damage)
		{
			SConsole.Log(LOG_TAG, $"Block: {gameObject.name} takes damage \nHP reduced from {_health} to {_health - damage}");
			_health -= damage;
			OnDamage?.Invoke(_health);
			
			SetModel();
			
			if (_health <= 0)
			{
				Destroy();
				return true;
			}
			
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