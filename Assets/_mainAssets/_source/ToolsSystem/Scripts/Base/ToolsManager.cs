using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace ToolsSystem
{
	[RequireComponent(typeof(AudioSource))]
	public class ToolsManager : MonoBehaviour
	{
		[SerializeField] private NearFarInteractor GrabHand;
		[SerializeField] private XRRayInteractor TeleportRay;
		
		// TODO: activate multiple tools in same time
		private Tool _activeTools;
		
		// TODO: move interaction filters to tools
		public void ActivateTool(Tool tool)
		{			
			if (tool == null) { return; }
			
			bool _canTeleport = tool.ToolInfo.InteractionMode.HasFlag(InToolInteractionMode.Teleport);
			bool _canGrab = tool.ToolInfo.InteractionMode.HasFlag(InToolInteractionMode.Grab);
			bool _canUI = tool.ToolInfo.InteractionMode.HasFlag(InToolInteractionMode.UI);
			
			TeleportRay.enabled = _canTeleport;
			
			// TODO: rework bug
			GrabHand.enableNearCasting = _canGrab;
			GrabHand.enableFarCasting = _canGrab;
			
			GrabHand.enableUIInteraction = _canUI;
			
			tool.ChangeToolActiveState(true);
			_activeTools = tool;
		}
		
		public void DeactivateTool()
		{
			if (_activeTools == null)
			{
				TeleportRay.enabled = true;
				GrabHand.enabled = true;
				GrabHand.enableUIInteraction = true;
				return;
			}
			
			_activeTools?.ChangeToolActiveState(false);
			_activeTools = null;
		}
		
		public void ForceSetTool(Tool tool)
		{
			if (!tool) { return; }
			
			switch (tool)
			{
				case PhysicTool physic:
					physic.ActivateAndForceGrabTool();
					break;
				default:
					ActivateTool(tool);
					break;
			}
		}
	}
}