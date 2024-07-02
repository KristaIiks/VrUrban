using System;

namespace MsgDisplay
{
	[Flags]
	public enum MsgCategoryEnum
	{
		Info = 0,
		Warning = 1,
		Error = 2,
		System = 4
	}
}