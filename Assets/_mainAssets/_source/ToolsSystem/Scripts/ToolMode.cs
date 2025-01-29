using System;

namespace ToolsSystem
{
	[Serializable, Flags]
	public enum ToolMode
	{
		Teleport,
		Grab,
		UI
	}
}
