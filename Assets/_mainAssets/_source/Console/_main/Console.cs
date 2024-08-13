using System;
using System.Reflection;
using UnityEngine;

namespace SmartConsole
{
	// TODO: create editor menu to configure
	// TODO: add filter to control display level in game + state
	// TODO: add scriptable settings
	public class Console
	{
		private const string LOG_TAG = "<color=#ffff00ff>Console</color>";
		private const int DISPLAY_NUM = 2;
		
		private static Logger _unityLogger = new Logger(Debug.unityLogger);
		private static SmartLogger _smartLogger = SmartLogger.Instance(_unityLogger, DISPLAY_NUM);
		private static bool _loggerEnabled = false;

		#if UNITY_EDITOR
		[UnityEditor.InitializeOnEnterPlayMode]
		#else
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		#endif
		private static void Init()
		{
			#if UNITY_EDITOR
			if(UnityEditor.SessionState.GetBool("EditorLogEnabled", false))
			 {			
				_smartLogger.ChangeFile(UnityEditor.SessionState.GetString("EditorLogPath", ""));
				
				_smartLogger.logEnabled = true;
				_smartLogger.Log(LOG_TAG, "Disable editor smart logger...");
				_smartLogger.logEnabled = false;
				
				UnityEditor.SessionState.SetString("EditorLogPath", "");
				UnityEditor.SessionState.SetBool("EditorLogEnabled", false);
			}
			#endif			
			EnableSmartLogger();
		}
		
		#if UNITY_EDITOR
		private static void InitEditor()
		{
			Init();
			UnityEditor.SessionState.SetString("EditorLogPath", _smartLogger.fileWriter.FilePath);
			UnityEditor.SessionState.SetBool("EditorLogEnabled", true);
		}
		
		[UnityEditor.InitializeOnLoadMethod]
		private static void SetupEditorLogging()
		{
			if (!UnityEditor.SessionState.GetBool("FirstSetup", false))
			{				
				InitEditor();
				UnityEditor.SessionState.SetBool("FirstSetup", true);
			}
		}
		#endif
		
		[HideInCallstack]
		public static void Log(string tag, object message, int logLevel = 1, UnityEngine.Object context = null)
		{
			if (_loggerEnabled)
			{
				_smartLogger.Log(tag, message, logLevel, context);
				return;
			}
			
			switch(_smartLogger.GetType(logLevel))
			{
				case LogType.Error:
					Debug.LogError(message, context);
					break;
				case LogType.Assert:
					Debug.LogAssertion(message, context);
					break;
				case LogType.Exception:
					LogException(tag, message as Exception, context);
					break;
				case LogType.Warning:
					Debug.LogWarning(message, context);
					break;	
				default:
					Debug.Log(message, context);
					break;
			}
		}
		[HideInCallstack]
		public static void Log(string tag, object message, LogType logType, UnityEngine.Object context = null) =>
			Log(tag, message, _smartLogger.GetLevel(logType), context);
		[HideInCallstack]
		public static void LogException(string tag, Exception exception, UnityEngine.Object context = null)
		{
			if(exception == null) { exception = new NullReferenceException(); }
			
			if(_loggerEnabled)
			{
				_smartLogger.Log(LogType.Exception, tag, exception, context);
				return;
			}
			
			Debug.LogException(exception, context);
		}
		
		public static void EnableSmartLogger()
		{
			if(_loggerEnabled) { return; }
			
			_smartLogger.GenerateFile();
			
			Log(LOG_TAG, @"Enable smart logger...");
			
			ChangeLogger(_smartLogger);
			Application.quitting += DisableSmartLogger;
			
			Log(LOG_TAG, $"Smart logger successful enabled!");
			PrintEnablingStats();
		}
		
		public static void DisableSmartLogger()
		{
			if(!_loggerEnabled) { return; }
			
			Log(LOG_TAG, "Disable smart logger...");
			Application.quitting -= DisableSmartLogger;
			
			#if UNITY_EDITOR
			_loggerEnabled = false;
			InitEditor();
			#else
			ChangeLogger(_unityLogger);
			#endif
		}
		
		private static void ChangeLogger(ILogger logger)
		{
			try
			{
				Type type = typeof(Debug);
				FieldInfo fieldInfo = type.GetField("s_Logger", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
				fieldInfo.SetValue(null, logger);

				_loggerEnabled = !_loggerEnabled;
				_smartLogger.logEnabled = _loggerEnabled;
				
				Log(LOG_TAG, "Logger successful changed!");
			}
			catch(Exception ex)
			{
				LogException(LOG_TAG, ex);
				Log(LOG_TAG, "Logger change failed!", LogType.Error);
			}
		}

		private static void PrintEnablingStats()
		{
			string text = "System stats\n\n";
			
			// TODO: add account info + server connection maybe move to other system logs
			text += $"App version: {Application.version}\n";
			text += $"Device type: {SystemInfo.deviceType}\n";
			text += $"Model: {SystemInfo.deviceModel}\n";
			text += $"Name: {SystemInfo.deviceName}\n";
			text += $"ID: {SystemInfo.deviceUniqueIdentifier}\n";
			text += $"OS: {SystemInfo.operatingSystem}\n";			
			text += $"Battery: {SystemInfo.batteryStatus} | {Mathf.FloorToInt(SystemInfo.batteryLevel * 100)}%\n\n";
			
			text += $"Graphics Device Type: {SystemInfo.graphicsDeviceType}\n";
			text += $"Graphics Device Name: {SystemInfo.graphicsDeviceName} | {SystemInfo.graphicsDeviceVendor}\n";
			text += $"Graphics Device ID: {SystemInfo.graphicsDeviceID}\n\n";
			
			text += $"Processor Name: {SystemInfo.processorType}\n";
			text += $"Processor Frequency: {SystemInfo.processorFrequency}Hz\n";
			text += $"Processors count: {SystemInfo.processorCount}";
			
			Log(LOG_TAG, text);
		}
	}
}