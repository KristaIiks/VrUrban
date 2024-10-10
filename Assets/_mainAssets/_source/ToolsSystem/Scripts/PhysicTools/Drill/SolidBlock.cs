using System;
using UnityEngine;
using SmartConsole;

namespace ToolsSystem
{
	public class SolidBlock : BaseToolObject
	{
		public const string OBJECT_TAG = "SolidBlock";
		private const string LOG_TAG = "Tool: DrillTool";
		
		[SerializeField] private float _health;
		
		public event Action OnDestroy;
		public SolidBlockSettings Settings;
		
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
			}
		}
		
		public bool ApplyDamage(float damage)
		{
			_health -= damage;
			
			if (_health <= 0)
			{
				Destroy();
				return true;
			}
			
			return false;
		}
		
		private void Destroy()
		{
			gameObject.SetActive(false);
			SConsole.Log(LOG_TAG, "SolidBlock destroyed: " + gameObject.name);
			OnDestroy?.Invoke();
		}
	}
}
