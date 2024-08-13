using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SmartConsole
{
	// TODO: add send event
	public class SmartLogger : ILogger, ILogHandler
	{
		private const string EDITOR_LOG_FORMAT = "<b>[{0}]#{1}:</b> {2}\n";
		private const string FILE_LOG_FORMAT = "[{1}] [{0}][{2}]";
		private const string DEFAULT_TAG = "<color=#808080ff>System</color>";
		
		private static SmartLogger Logger;
		
		private Logger _unityLogger;
		private ILogHandler _loggerHandler;
		private FileWriter _fileWriter;
		private bool _logEnabled;
		
		/* 0 - 11

			0 - Default
			1 - Full info
			2 - Info
			3-6 - Warning
			7 - Exception
			8 - Assert
			9-10 - Error
			11 - Disabled
			
		*/
		private int _displayFilter;
		
		// TODO: change this to data
		private string _lastLog;
		private string _lastLogText;
		private int _collapseCount;
		
		protected SmartLogger(Logger unityLogger, int displayFilter)
		{
			_displayFilter = Mathf.Clamp(displayFilter, 0, 11);
			_loggerHandler = this;
			_unityLogger = unityLogger;
		}
		
		public static SmartLogger Instance(Logger unityLogger, int displayNum = 0)
		{
			if(Logger == null)
			{
				Logger = new SmartLogger(unityLogger, displayNum);
			}
			
			return Logger;
		}
		
		public void GenerateFile()
		{
			string path = "";
			
			#if UNITY_EDITOR
			if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
			{
				path = UnityEditor.EditorApplication.isPlaying ? @$"{Application.persistentDataPath}\Logs\Editor" : @$"{Application.persistentDataPath}\Logs";
			}
			else
			{
				path = @$"{Application.persistentDataPath}\Logs\Editor";
			}
			#else
			path = @$"{Directory.GetCurrentDirectory()}\Logs";
			#endif
			
			Directory.CreateDirectory(path);
			_fileWriter = new FileWriter(path, Guid.NewGuid().ToString());
		}
		
		#if UNITY_EDITOR
		public void ChangeFile(string newFilePath)
		{
			if(!File.Exists(newFilePath)) { return; }
			
			_fileWriter = new FileWriter(newFilePath);
		}
		#endif
			
		public ILogHandler logHandler
		{
			get => _loggerHandler;
			set => _loggerHandler = value;
		}
		public bool logEnabled
		{
			get => _logEnabled;
			set => _logEnabled = value;
		}
		public LogType filterLogType
		{
			get => GetType(_displayFilter);
			set => _displayFilter = GetLevel(value);
		}
		public FileWriter fileWriter
		{
			get => _fileWriter;
		}

		[HideInCallstack]
		public int GetLevel(LogType logType) => logType switch
		{
			LogType.Warning => 3,
			LogType.Exception => 7,
			LogType.Assert => 8,
			LogType.Error => 9,
			_ => 0,
		};
		
		[HideInCallstack]
		public LogType GetType(int logType) => logType switch
		{
			int x when x > 8 => LogType.Error,
			int x when x == 8 => LogType.Assert,
			int x when x == 7 => LogType.Exception,
			int x when x >= 3 => LogType.Warning,
			_ => LogType.Log
		};
		
		[HideInCallstack]
		public bool IsLogTypeAllowed(LogType logType) => GetLevel(logType) >= _displayFilter;
		
		[HideInCallstack]
		public void Log(string tag, object message, int logLevel = 1, Object context = null)
		{
			if(!_logEnabled || tag == "") { return; }
			
			string text = GetString(message);
			logLevel = Mathf.Clamp(logLevel, 0, 10);
			
			if(logLevel >= _displayFilter)
			{
				_unityLogger.logHandler.LogFormat(GetType(logLevel), context, EDITOR_LOG_FORMAT, tag, logLevel, text);
			}
			
			if(_lastLog == tag + text + logLevel + context)
			{
				_collapseCount++;
				_fileWriter.RemoveText(Encoding.UTF8.GetByteCount(_lastLogText));
			}
			else
			{
				_lastLog = tag + text + logLevel + context;
				_collapseCount = 1;
			}
			
			FormatText(tag, ref text, logLevel);
			_lastLogText = text;
			
			_fileWriter.Print(text);
		}
		
		[HideInCallstack] public void Log(LogType logType, object message) => Log(DEFAULT_TAG, message, GetLevel(logType));
		[HideInCallstack] public void Log(LogType logType, object message, Object context) => Log(DEFAULT_TAG, message, GetLevel(logType), context);
		[HideInCallstack] public void Log(LogType logType, string tag, object message) =>  Log(tag, message, GetLevel(logType));
		[HideInCallstack] public void Log(LogType logType, string tag, object message, Object context) =>  Log(tag, message, GetLevel(logType), context);
		
		[HideInCallstack] public void Log(object message) => Log(LogType.Log, message);
		[HideInCallstack] public void Log(string tag, object message) =>  Log(tag, message, 0);
		[HideInCallstack] public void Log(string tag, object message, Object context) =>  Log(tag, message, 0, context);
		
		[HideInCallstack] public void LogWarning(string tag, object message) => Log(tag, message, GetLevel(LogType.Warning));
		[HideInCallstack] public void LogWarning(string tag, object message, Object context) => Log(tag, message, GetLevel(LogType.Warning), context);

		[HideInCallstack] public void LogException(Exception exception) => Log(LogType.Exception, exception.Message);
		[HideInCallstack] public void LogException(Exception exception, Object context) => Log(LogType.Exception, exception, context);
		
		[HideInCallstack] public void LogError(string tag, object message) => Log(tag, message, GetLevel(LogType.Error));
		[HideInCallstack] public void LogError(string tag, object message, Object context) => Log(tag, message, GetLevel(LogType.Error), context);
		
		[HideInCallstack] public void LogFormat(LogType logType, string format, params object[] args) => Log(logType, string.Format(format, args));
		[HideInCallstack] public void LogFormat(LogType logType, Object context, string format, params object[] args) => Log(logType, string.Format(format, args) as object, context);
		
		[HideInCallstack]
		private string GetString(object message)
		{
			if (message == null)
			{
				return "Null";
			}

			if (message is IFormattable formattable)
			{
				return formattable.ToString(null, CultureInfo.InvariantCulture);
			}

			return message.ToString();
		}
		
		private void FormatText(string tag, ref string text, int logLevel)
		{
			text = text.Replace("\r\n", "\n");
			string[] lines = text.Split('\n');
			text = "";
			
			if(lines.Length > 1)
			{
				text += Regex.Replace(string.Format(FILE_LOG_FORMAT, tag, DateTime.Now.ToString("HH:mm:ss"), _collapseCount), "<.*?>", String.Empty).PadRight(26, ' ') + $"⋘{logLevel}⋙     ▼" + Environment.NewLine;
				text += "".PadRight(37, ' ') + "┓" + Environment.NewLine;
				
				for (int i = 0; i < lines.Length; i++)
				{
					
					text += Regex.Replace("".PadRight(37, ' ') + $"┃ {lines[i]}", "<.*?>", String.Empty) + Environment.NewLine;
					
				}
				text += "".PadRight(37, ' ') + "┛";
			}
			else
			{
				text = Regex.Replace(string.Format(FILE_LOG_FORMAT, tag, DateTime.Now.ToString("HH:mm:ss"), _collapseCount), "<.*?>", String.Empty).PadRight(26, ' ') + $"⋘{logLevel}⋙     │ {lines[0]}";
			}
		}
	}
}