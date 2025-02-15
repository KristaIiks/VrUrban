using System;
using SmartConsole;
using UnityEngine;

namespace ToolsSystem
{
	[RequireComponent(typeof(AudioSource))]
	public abstract class Tool : MonoBehaviour
	{
		[field:SerializeField] public ToolInfo ToolInfo { get; private set; }
		public bool IsEnabled { get; private set; }
		
		public event Action<bool> OnToolActiveStateChanged;
		public event Action<bool> OnToolSelectChanged;
		
		protected string LOG_TAG { get { return $"Tool: {ToolInfo?.ToolName ?? "ErrorName"}"; } }
		protected AudioSource _audio;
		
		protected virtual void OnValidate()
		{
			_audio ??= GetComponent<AudioSource>();
		}

		public virtual void ChangeToolActiveState(bool state)
		{
			IsEnabled = state;
			OnToolActiveStateChanged?.Invoke(state);
			SConsole.Log(LOG_TAG, "Active state changed: " + state);
		}
		
		protected virtual void SelectTool(bool state)
		{			
			if (ToolInfo == null) { SConsole.LogException(LOG_TAG, new NullReferenceException("ToolInfo is null"), this); return; }

			AudioClip clipToPlay = state ? ToolInfo.SelectToolClip : ToolInfo.DeSelectToolClip;
			
			if (clipToPlay)
				_audio.PlayOneShot(clipToPlay);
			OnToolSelectChanged?.Invoke(state);
			
			SConsole.Log(LOG_TAG, $"Select tool changed: {state}");
		
		}
	}
}