using System;
using UnityEngine;

namespace Logging
{
	public class Logger: ILogHandler
	{
		private ILogHandler _defaultLogHandler;
		
		public void EnableLogger()
		{
			_defaultLogHandler ??= Debug.unityLogger.logHandler;
			Debug.unityLogger.logHandler = this;
		}
		
		public void DisableLogger()
		{
			Debug.unityLogger.logHandler = _defaultLogHandler;
		}		
		
		/// <summary>
		/// Log unsorted message
		/// </summary>
		public void Log(string msg, string tag = "DEFAULT", bool console = true)
		{
			if (console) { Debug.Log(msg); }
			
			//TODO: save
		}
		
		/// <summary>
		/// Log message with tag
		/// </summary>
		public void Log(string msg, string tag)
		{
			Log($"[{tag}] {msg}");
		}
		
		public void LogWarning(string msg, string tag)
		{
			Log($"<color=yellow>{msg}</color>", $"[<color=yellow>Warning</color>] {tag}");
		}
				
		//https://docs.unity3d.com/ScriptReference/ILogHandler.html

		public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
		{
			_defaultLogHandler.LogFormat(logType, context, format, args);
			//TODO: save
		}

		public void LogException(Exception exception, UnityEngine.Object context)
		{
			_defaultLogHandler.LogException(exception, context);
		}

		/* public static void Log(string msg, string _tag = "", LogType _type = LogType.Log)
		{
			if(_type == LogType.Log || _type == LogType.Assert) { Debug.Log(msg); return; }
			if(_type == LogType.Error || _type == LogType.Exception) { Debug.LogError(msg); return; }
		} */
	}
}