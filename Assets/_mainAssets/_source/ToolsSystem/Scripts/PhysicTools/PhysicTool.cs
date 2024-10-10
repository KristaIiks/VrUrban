using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace ToolsSystem
{
	[RequireComponent(typeof(XRGrabInteractable))]
	public abstract class PhysicTool : Tool
	{
		// TODO: realize permanent grab if (maybe create my own interactor)
		[SerializeField] private bool _forceInteract;
		
		protected XRGrabInteractable _grabInteractable;
		protected bool _isGrabbed;

		protected override void OnValidate()
		{
			base.OnValidate();
			if(_grabInteractable == null)
			{
				_grabInteractable = GetComponent<XRGrabInteractable>();
				_grabInteractable.enabled = false;
			}
		}
		
		protected virtual void OnEnable()
		{
			_grabInteractable.selectEntered.AddListener(ActivateTool);
			_grabInteractable.selectExited.AddListener(ActivateTool);
		}
		
		protected virtual void OnDisable()
		{
			_grabInteractable.selectEntered.RemoveListener(ActivateTool);
			_grabInteractable.selectExited.RemoveListener(ActivateTool);
		}

		public override void ChangeToolActiveState(bool state)
		{
			base.ChangeToolActiveState(state);
			_grabInteractable.enabled = IsEnabled;
		}

		protected override void SelectTool(bool state)
		{
			base.SelectTool(state);
			_isGrabbed = state;
		}

		public void ActivateAndGrabTool()
		{
			ChangeToolActiveState(true);
			// TODO: Grab 
		}

		// Activate/deactivate event on pick/drop item
		private void ActivateTool(SelectEnterEventArgs arg) => SelectTool(true);
		private void ActivateTool(SelectExitEventArgs arg) => SelectTool(false);
	}
}
