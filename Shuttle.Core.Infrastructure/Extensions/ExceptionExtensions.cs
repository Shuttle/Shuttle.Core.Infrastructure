using System;
using System.Collections.Generic;
using System.Text;

namespace Shuttle.Core.Infrastructure
{
	public static class ExceptionExtensions
	{
		public static IList<string> Messages(this Exception ex)
		{
			var messages = new List<string>();

			var i = 0;

			var enumerator = ex;

			while (enumerator != null)
			{
				messages.Add(string.Format("{0}{1}", new string(' ', i) + ((i > 0)
																			   ? "+->"
																			   : string.Empty), enumerator.Message));

				enumerator = enumerator.InnerException;

				i++;
			}

			return messages;
		}

		public static string AllMessages(this Exception ex)
		{
			var messages = new StringBuilder();

			var enumerator = ex;

			while (enumerator != null)
			{
				messages.AppendFormat("{0}{1}", messages.Length > 0 ? " / " : string.Empty, enumerator.Message);

				enumerator = enumerator.InnerException;
			}

			return messages.ToString();
		}

		public static Exception TrimLeading<T>(this Exception ex) where T : Exception
		{
			Guard.AgainstNull(ex, "ex");

			var trim = typeof(T);

			var exception = ex;

			while (exception.GetType() == trim)
			{
				if (exception.InnerException == null)
				{
					break;
				}

				exception = exception.InnerException;
			}

			return exception;
		}
	}
}
