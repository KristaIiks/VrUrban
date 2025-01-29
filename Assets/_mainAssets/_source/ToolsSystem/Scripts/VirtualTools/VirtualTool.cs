namespace ToolsSystem
{
	public abstract class VirtualTool : Tool
	{						
		public override void ChangeToolActiveState(bool state)
		{
			base.ChangeToolActiveState(state);
			SelectTool(state);
		}

		protected override void SelectTool(bool state)
		{
			base.SelectTool(state);
			gameObject.SetActive(state);
		}
	}
}