using System;
using SmartConsole;
using UnityEngine;

namespace ToolsSystem
{
	[RequireComponent(typeof(AudioSource))]
	public abstract class Tool : MonoBehaviour
	{
		[field:SerializeField] public ToolInfo ToolInfo { get; private set; }
		[SerializeField] protected AudioSource Audio;
		
		public bool IsEnabled { get; private set; }
		
		public event Action<bool> OnToolActiveStateChanged;
		public event Action<bool> OnToolSelectChanged;
		
		protected string LOG_TAG { get { return $"Tool: {ToolInfo?.ToolName ?? "ErrorName"}"; } }
		
		protected virtual void OnValidate()
		{
			Audio ??= GetComponent<AudioSource>();
		}

		/// <summary>
		/// Change can we take this tool
		/// </summary>
		/// <param name="state"></param>
		public virtual void ChangeToolActiveState(bool state)
		{
			IsEnabled = state;
			SConsole.Log(LOG_TAG, "Active state changed: " + state);
			OnToolActiveStateChanged?.Invoke(state);
		}
		
		/// <summary>
		/// Take tool in hand
		/// </summary>
		/// <param name="state"></param>
		protected virtual void SelectTool(bool state)
		{			
			if (ToolInfo == null) { SConsole.Log(LOG_TAG, "ToolInfo is null", LogType.Error, this); }

			AudioClip clipToPlay = state ? ToolInfo.SelectToolClip : ToolInfo.DeSelectToolClip;
			
			if (clipToPlay)
				Audio.PlayOneShot(clipToPlay);
				
			SConsole.Log(LOG_TAG, $"Select tool changed: {state}");
			OnToolSelectChanged?.Invoke(state);
		}
	}
}