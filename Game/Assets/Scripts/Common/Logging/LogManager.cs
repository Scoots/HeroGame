using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace Common.Logging
{
	public class LogManager
	{
		public static bool IsEnabled { get; set; }

		internal readonly static List<LogTarget> Targets = new List<LogTarget>();
		internal readonly static Dictionary<string, Logger> Loggers = new Dictionary<string, Logger>();

		public static Logger CreateLog()
		{
			var stackFrame = new StackFrame(1, false);
			var className = stackFrame.GetMethod().DeclaringType.Name;

			if (className == null)
				throw new Exception("Error getting full name for declaring type.");

			if (!Loggers.ContainsKey(className))
				Loggers.Add(className, new Logger(className));

			return Loggers[className];
		}

		public static Logger CreateLog(string inName)
		{
			if (!Loggers.ContainsKey(inName))
				Loggers.Add(inName, new Logger(inName));

			return Loggers[inName];
		}

		public static void AttachLogTarget(LogTarget target)
		{
			Targets.Add(target);
		}
	}
}
