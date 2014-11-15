using System;

namespace Shuttle.Core.Infrastructure
{
	public abstract class AbstractLog : ILog
	{
		protected AbstractLog()
		{
			LogLevel = LogLevel.All;
		}

		public void AtLevel(LogLevel level, string message)
		{
			switch (level)
			{
				case LogLevel.Verbose:
					{
						Verbose(message);
						break;
					}
				case LogLevel.Trace:
					{
						Trace(message);
						break;
					}
				case LogLevel.Debug:
					{
						Debug(message);
						break;
					}
				case LogLevel.Information:
					{
						Information(message);
						break;
					}
				case LogLevel.Warning:
					{
						Warning(message);
						break;
					}
				case LogLevel.Error:
					{
						Error(message);
						break;
					}
				case LogLevel.Fatal:
					{
						Fatal(message);
						break;
					}
			}
		}

		public abstract void Verbose(string message);
		public abstract void Trace(string message);
		public abstract void Debug(string message);
		public abstract void Warning(string message);
		public abstract void Information(string message);
		public abstract void Error(string message);
		public abstract void Fatal(string message);

		public void Verbose(bool condition, string message)
		{
			if (!condition)
			{
				return;
			}

			Verbose(message);
		}

		public void Trace(bool condition, string message)
		{
			if (!condition)
			{
				return;
			}

			Trace(message);
		}

		public void Debug(bool condition, string message)
		{
			if (!condition)
			{
				return;
			}

			Debug(message);
		}

		public void Information(bool condition, string message)
		{
			if (!condition)
			{
				return;
			}

			Information(message);
		}

		public void Warning(bool condition, string message)
		{
			if (!condition)
			{
				return;
			}

			Warning(message);
		}

		public void Error(bool condition, string message)
		{
			if (!condition)
			{
				return;
			}

			Error(message);
		}

		public void Fatal(bool condition, string message)
		{
			if (!condition)
			{
				return;
			}

			Fatal(message);
		}

		public void Verbose(Func<bool> condition, string message)
		{
			Guard.AgainstNull(condition, "condition");

			Verbose(condition(), message);
		}

		public void Trace(Func<bool> condition, string message)
		{
			Guard.AgainstNull(condition, "condition");

			Trace(condition(), message);
		}

		public void Debug(Func<bool> condition, string message)
		{
			Guard.AgainstNull(condition, "condition");

			Debug(condition(), message);
		}

		public void Information(Func<bool> condition, string message)
		{
			Guard.AgainstNull(condition, "condition");

			Information(condition(), message);
		}

		public void Warning(Func<bool> condition, string message)
		{
			Guard.AgainstNull(condition, "condition");

			Warning(condition(), message);
		}

		public void Error(Func<bool> condition, string message)
		{
			Guard.AgainstNull(condition, "condition");

			Error(condition(), message);
		}

		public void Fatal(Func<bool> condition, string message)
		{
			Guard.AgainstNull(condition, "condition");

			Fatal(condition(), message);
		}

		public LogLevel LogLevel { get; protected set; }

        public virtual bool IsEnabled(LogLevel level)
        {
            return LogLevel <= level;
        }

	    public virtual bool IsVerboseEnabled
		{
			get { return LogLevel < LogLevel.Trace; }
		}

		public virtual bool IsTraceEnabled
		{
			get { return LogLevel < LogLevel.Debug; }
		}

		public virtual bool IsDebugEnabled
		{
			get { return LogLevel < LogLevel.Information; }
		}

		public virtual bool IsInformationEnabled
		{
			get { return LogLevel < LogLevel.Warning; }
		}

		public virtual bool IsWarningEnabled
		{
			get { return LogLevel < LogLevel.Error; }
		}

		public virtual bool IsErrorEnabled
		{
			get { return LogLevel < LogLevel.Fatal; }
		}

		public virtual bool IsFatalEnabled
		{
			get { return true; }
		}

		public abstract ILog For(Type type);
		public abstract ILog For(object instance);
	}
}