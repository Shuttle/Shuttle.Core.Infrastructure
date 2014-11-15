using System;
using System.Threading;

namespace Shuttle.Core.Infrastructure
{
	public class ConsoleLog : AbstractLog
	{
		private Type logtype;

		public ConsoleLog(Type type)
		{
			Guard.AgainstNull(type, "type");

			logtype = type;
		}

		public override void Verbose(string message)
		{
			if (!IsVerboseEnabled)
			{
				return;
			}

			WriteLog(ConsoleColor.DarkGray, "VERBOSE", message);
		}

		private void WriteLog(ConsoleColor color, string level, string message)
		{
			ColoredConsole.WriteLine(color, "{0} [{1}] {2} {3} - {4}", now(), Thread.CurrentThread.ManagedThreadId, string.Format("{0, -7}", level), logtype.FullName, message);
		}

		public override void Trace(string message)
		{
			if (!IsTraceEnabled)
			{
				return;
			}

			WriteLog(ConsoleColor.Gray, "TRACE", message);
		}

		public override void Debug(string message)
		{
			if (!IsDebugEnabled)
			{
				return;
			}

			WriteLog(ConsoleColor.Magenta, "DEBUG", message);
		}

		private static string now()
		{
			return DateTime.Now.ToString("yyyy/mm/dd HH:mm:ss.fff");
		}

		public override void Warning(string message)
		{
			if (!IsWarningEnabled)
			{
				return;
			}

			WriteLog(ConsoleColor.Yellow, "WARNING", message);
		}

		public override void Information(string message)
		{
			if (!IsInformationEnabled)
			{
				return;
			}

			WriteLog(ConsoleColor.White, "INFO", message);
		}

		public override void Error(string message)
		{
			if (!IsErrorEnabled)
			{
				return;
			}

			WriteLog(ConsoleColor.Red, "ERROR", message);
		}

		public override void Fatal(string message)
		{
			if (!IsFatalEnabled)
			{
				return;
			}

			WriteLog(ConsoleColor.DarkRed, "FATAL", message);
		}

		public new LogLevel LogLevel
		{
			get { return base.LogLevel; }
			set { base.LogLevel = value; }
		}

		public override ILog For(Type type)
		{
			Guard.AgainstNull(type, "type");

			return new ConsoleLog(type) { LogLevel = LogLevel };
		}

		public override ILog For(object instance)
		{
			Guard.AgainstNull(instance, "instance");

			return For(instance.GetType());
		}
	}
}