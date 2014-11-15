using System;

namespace Shuttle.Core.Infrastructure
{
	public static class Log
	{
		private static ILog _log;

		public static void Assign(ILog instance)
		{
			Guard.AgainstNull(instance, "instance");

			_log = instance;
		}

		public static TransientLog AssignTransient(ILog instance)
		{
			try
			{
				return new TransientLog(_log ?? NullLog.Instance);
			}
			finally
			{
				Assign(instance);
			}
		}

		public static void AtLevel(LogLevel level, string message)
		{
			GuardedLog().AtLevel(level, message);
		}

		public static void Verbose(string message)
		{
			GuardedLog().Verbose(message);
		}

		public static void Trace(string message)
		{
			GuardedLog().Trace(message);
		}

		public static void Debug(string message)
		{
			GuardedLog().Debug(message);
		}

		public static void Information(string message)
		{
			GuardedLog().Information(message);
		}

		public static void Warning(string message)
		{
			GuardedLog().Warning(message);
		}

		public static void Error(string message)
		{
			GuardedLog().Error(message);
		}

		public static void Fatal(string message)
		{
			GuardedLog().Fatal(message);
		}

		public static void Verbose(bool condition, string message)
		{
			if (!condition)
			{
				return;
			}

			Trace(message);
		}

		public static void Trace(bool condition, string message)
		{
			if (!condition)
			{
				return;
			}

			Trace(message);
		}

		public static void Debug(bool condition, string message)
		{
			if (!condition)
			{
				return;
			}

			Debug(message);
		}

		public static void Information(bool condition, string message)
		{
			if (!condition)
			{
				return;
			}

			Information(message);
		}

		public static void Warning(bool condition, string message)
		{
			if (!condition)
			{
				return;
			}

			Warning(message);
		}

		public static void Error(bool condition, string message)
		{
			if (!condition)
			{
				return;
			}

			Error(message);
		}

		public static void Fatal(bool condition, string message)
		{
			if (!condition)
			{
				return;
			}

			Fatal(message);
		}

		public static void Verbose(Func<bool> condition, string message)
		{
			Guard.AgainstNull(condition, "condition");

			Verbose(condition(), message);
		}

		public static void Trace(Func<bool> condition, string message)
		{
			Guard.AgainstNull(condition, "condition");

			Trace(condition(), message);
		}

		public static void Debug(Func<bool> condition, string message)
		{
			Guard.AgainstNull(condition, "condition");

			Debug(condition(), message);
		}

		public static void Information(Func<bool> condition, string message)
		{
			Guard.AgainstNull(condition, "condition");

			Information(condition(), message);
		}

		public static void Warning(Func<bool> condition, string message)
		{
			Guard.AgainstNull(condition, "condition");

			Warning(condition(), message);
		}

		public static void Error(Func<bool> condition, string message)
		{
			Guard.AgainstNull(condition, "condition");

			Error(condition(), message);
		}

		public static void Fatal(Func<bool> condition, string message)
		{
			Guard.AgainstNull(condition, "condition");

			Fatal(condition(), message);
		}

		public static ILog For(Type type)
		{
			return GuardedLog().For(type);
		}

		public static bool IsVerboseEnabled
		{
			get { return GuardedLog().IsVerboseEnabled; }
		}

		public static bool IsTraceEnabled
		{
			get { return GuardedLog().IsTraceEnabled; }
		}

		public static bool IsDebugEnabled
		{
			get { return GuardedLog().IsDebugEnabled; }
		}

		public static bool IsInformationEnabled
		{
			get { return GuardedLog().IsInformationEnabled; }
		}

		public static bool IsWarningEnabled
		{
			get { return GuardedLog().IsWarningEnabled; }
		}

		public static bool IsErrorEnabled
		{
			get { return GuardedLog().IsErrorEnabled; }
		}

		public static bool IsFatalEnabled
		{
			get { return GuardedLog().IsFatalEnabled; }
		}

		public static ILog For(object instance)
		{
			return GuardedLog().For(instance);
		}

		private static ILog GuardedLog()
		{
			return _log ?? NullLog.Instance;
		}
	}

	public class TransientLog : IDisposable
	{
		private readonly ILog _previousLog;


		public TransientLog(ILog previousLog)
		{
			_previousLog = previousLog;
		}


		public void Dispose()
		{
			Log.Assign(_previousLog);
		}
	}
}