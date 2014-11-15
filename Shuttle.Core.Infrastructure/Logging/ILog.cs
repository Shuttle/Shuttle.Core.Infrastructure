using System;

namespace Shuttle.Core.Infrastructure
{
    public enum LogLevel
    {
		Verbose = 1,
		Trace = 2,
        Debug = 3,
        Information = 4,
        Warning = 5,
        Error = 6,
        Fatal = 7,
        All = 1,
        Off = 7,
        None = 7
    }

    public interface ILog
    {
        void AtLevel(LogLevel level, string message);

        void Verbose(string message);
        void Trace(string message);
        void Debug(string message);
        void Information(string message);
        void Warning(string message);
        void Error(string message);
        void Fatal(string message);

		void Verbose(bool condition, string message);
		void Trace(bool condition, string message);
		void Debug(bool condition, string message);
		void Information(bool condition, string message);
		void Warning(bool condition, string message);
		void Error(bool condition, string message);
		void Fatal(bool condition, string message);

		void Verbose(Func<bool> condition, string message);
		void Trace(Func<bool> condition, string message);
		void Debug(Func<bool> condition, string message);
		void Information(Func<bool> condition, string message);
		void Warning(Func<bool> condition, string message);
		void Error(Func<bool> condition, string message);
		void Fatal(Func<bool> condition, string message);

        LogLevel LogLevel { get; }

        bool IsEnabled(LogLevel level);
        
        bool IsVerboseEnabled { get; }
        bool IsTraceEnabled { get; }
        bool IsDebugEnabled { get; }
        bool IsInformationEnabled { get; }
        bool IsWarningEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }

        ILog For(Type type);
        ILog For(object instance);
    }
}