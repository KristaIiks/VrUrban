using System;
using UnityEngine;
using SmartConsole;

namespace ToolsSystem
{
	public sealed class SolidBlock : BaseToolObject
	{
		public const string OBJECT_TAG = "SolidBlock";
		private const string LOG_TAG = "Tool: DrillTool";
		
		[SerializeField] private float _health;
		[SerializeField] private GameObject[] _blockStages;
		
		public event Action<float> OnDamage;
		public event Action OnDestroy;
		public SolidBlockSettings Settings;
		
		private float _stageHealth { get => Settings.Health / _blockStages.Length; }
		
		private void OnValidate()
		{
			gameObject.tag = OBJECT_TAG;
		}
		
		private void Awake()
		{
			if (!Settings)
			{
				SConsole.LogException(LOG_TAG, new NullReferenceException(), Settings);
				Destroy(gameObject);
			}
			else
			{
				_health = Settings.Health;
				SetModel();
			}
		}
		
		public bool ApplyDamage(float damage)
		{
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
			SConsole.Log(LOG_TAG, "SolidBlock destroyed: " + gameObject.name);
			OnDestroy?.Invoke();
		}
	}
}
