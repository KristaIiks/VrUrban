using System;
using UnityEngine;

namespace ToolsSystem
{
	[RequireComponent(typeof(AudioSource))]
	public class ToolsManager : MonoBehaviour
	{		
		public void ActivateTool(Tool tool)
		{
			tool.ChangeToolActiveState(true);
		}
		
		public void ForceSetTool(Tool tool)
		{
			switch (tool)
			{
				case PhysicTool physic:
					physic.ActivateAndGrabTool();
					break;
				default:
					ActivateTool(tool);
					break;
			}
		}
	}
}