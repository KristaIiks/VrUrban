using System;

namespace ToolsSystem
{
	[Flags]
	public enum ToolMode
	{
		Teleport = 1,
		Grab = 2,
		UI = 4
	}
}
