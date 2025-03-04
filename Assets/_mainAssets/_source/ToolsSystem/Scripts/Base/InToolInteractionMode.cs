using System;

namespace ToolsSystem
{
	[Flags]
	public enum InToolInteractionMode
	{
		Teleport = 1,
		Grab = 2,
		UI = 4
	}
}