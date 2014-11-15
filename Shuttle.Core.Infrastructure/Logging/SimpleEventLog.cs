using System;

namespace Shuttle.Core.Infrastructure
{
	public class SimpleEventLog : AbstractLog
	{
		public Type logtype;

		public SimpleEventLog(Type type)
		{
			Guard.AgainstNull(type, "type");

			logtype = type;
		}

		public static event LoggerDelegate LogFine = delegate { };
		public static event LoggerDelegate LogTrace = delegate { };
		public static event LoggerDelegate LogDebug = delegate { };
		public static event LoggerDelegate LogInformation = delegate { };
		public static event LoggerDelegate LogWarning = delegate { };
		public static event LoggerDelegate LogError = delegate { };
		public static event LoggerDelegate LogFatal = delegate { };

		public override void Verbose(string message)
		{
			LogFine.Invoke(logtype, message);
		}

		public override void Trace(string message)
		{
			LogTrace.Invoke(logtype, message);
		}

		public override void Debug(string message)
		{
			LogDebug.Invoke(logtype, message);
		}

		public override void Information(string message)
		{
			LogInformation.Invoke(logtype, message);
		}

		public override void Error(string message)
		{
			LogError.Invoke(logtype, message);
		}

		public override void Fatal(string message)
		{
			LogFatal.Invoke(logtype, message);
		}

		public override ILog For(Type type)
		{
			Guard.AgainstNull(type, "type");

			return new SimpleEventLog(type) { LogLevel = LogLevel };
		}

		public override ILog For(object instance)
		{
			Guard.AgainstNull(instance, "instance");

			return For(instance.GetType());
		}

		public override void Warning(string message)
		{
			LogWarning.Invoke(logtype, message);
		}
	}

	public delegate void LoggerDelegate(Type type, string message);
}