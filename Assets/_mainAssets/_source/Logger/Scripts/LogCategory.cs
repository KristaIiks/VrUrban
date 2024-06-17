using UnityEngine;

namespace Logging
{
	public class LogCategory
	{
		private string _tag;
		
		public LogCategory(string Name) => _tag = Name;
		
		public void Log(string _msg, LogType _logType = LogType.Log) => Logger.Log(_msg, _tag);
	}
}