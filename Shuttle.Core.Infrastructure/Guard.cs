using System;
using System.Globalization;

namespace Shuttle.Core.Infrastructure
{
	public class Guard
	{
		public static void Against<TException>(bool assertion, string message) where TException : Exception
		{
			if (!assertion)
			{
				return;
			}

			Exception exception;

			try
			{
				exception = (TException) Activator.CreateInstance(typeof (TException), message);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(string.Format(InfrastructureResources.InvalidGuardExceptionType,
				                                                  typeof (TException).FullName, ex.AllMessages()));
			}

			throw exception;
		}

		public static void Against<TException>(Func<bool> assertion, string message) where TException : Exception
		{
			Against<TException>(assertion(), message);
		}

		public static void AgainstNull(object value, string name)
		{
			if (value == null)
			{
				throw new NullReferenceException(string.Format(CultureInfo.CurrentCulture,
				                                               InfrastructureResources.NullValueException,
				                                               name));
			}
		}

		public static void AgainstNullOrEmptyString(string value, string name)
		{
			AgainstNull(value, name);

			if (value.Length == 0)
				throw new EmptyStringException(string.Format(CultureInfo.CurrentCulture, InfrastructureResources.EmptyStringException, name));
		}

		public static void AgainstReassignment(object variable, string name)
		{
			if (variable == null)
			{
				return;
			}

			throw new ReassignmentException(string.Format(InfrastructureResources.ReassignmentException, name));
		}
	}
}