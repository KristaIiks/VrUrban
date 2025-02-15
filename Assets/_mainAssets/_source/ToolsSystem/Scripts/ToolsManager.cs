using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace ToolsSystem
{
	[RequireComponent(typeof(AudioSource))]
	public class ToolsManager : MonoBehaviour
	{
		[SerializeField] private NearFarInteractor GrabHand;
		[SerializeField] private XRRayInteractor TeleportRay;
		[SerializeField] private XRRayInteractor UIRay;
		
		private Tool _selectedTool;
		
		public void ActivateTool(Tool tool)
		{
			if (_selectedTool == tool) { return; }
			DeactivateTool();
			
			if (tool == null) { return; }
			
			bool _canTeleport = tool.ToolInfo.ToolMode.HasFlag(ToolMode.Teleport);
			bool _canGrab = !(tool is PhysicTool || !tool.ToolInfo.ToolMode.HasFlag(ToolMode.Grab));
			bool _canUI = tool.ToolInfo.ToolMode.HasFlag(ToolMode.UI);
			
			TeleportRay.enabled = _canTeleport;
			GrabHand.enabled = _canGrab;
			UIRay.enabled = _canUI;
			
			tool.ChangeToolActiveState(true);
			_selectedTool = tool;
		}
		
		public void DeactivateTool()
		{
			if (_selectedTool == null)
			{
				TeleportRay.enabled = true;
				GrabHand.enabled = true;
				UIRay.enabled = true;
				return;
			}
			
			_selectedTool?.ChangeToolActiveState(false);
			_selectedTool = null;
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