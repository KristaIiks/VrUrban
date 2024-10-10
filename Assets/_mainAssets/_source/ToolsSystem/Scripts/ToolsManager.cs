using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ToolsSystem
{
	[RequireComponent(typeof(AudioSource))]
	public static class ToolsManager// : MonoBehaviour
	{
		public static Action<Tool> OnSelectTool;
		private static Tool _selectedTool;
	
		
		
		public static void ActivateTool(Tool tool)
		{
			tool.ChangeToolActiveState(true);
		}
		
		public static void ForceSetTool(Tool tool)
		{
			switch (tool)
			{
				case PhysicTool:
					((PhysicTool)tool).ActivateAndGrabTool();
					break;
				default:
					ActivateTool(tool);
					break;
			}
		}

	}
}
