using UnityEngine;

namespace ToolsSystem
{
	[RequireComponent(typeof(AudioSource))]
	public class ToolsManager : MonoBehaviour
	{
		[SerializeField] private Tool _testTool;
		
		private void Awake()
		{
			ActivateTool(_testTool);
		}
		
		public void ActivateTool(Tool tool)
		{
			tool?.ChangeToolActiveState(true);
		}
		
		public void ForceSetTool(Tool tool)
		{
			if (!tool) { return; }
			
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