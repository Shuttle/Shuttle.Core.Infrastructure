using System;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
	public class ConfigurationItem<T>
	{
		private readonly T item;

		public ConfigurationItem(T item)
		{
			this.item = item;
		}

		public T GetValue()
		{
			return item;
		}

		public static ConfigurationItem<T> ReadSetting(string key)
		{
			return ReadSetting(key, true, default(T));
		}

		public static ConfigurationItem<T> ReadSetting(string key, T @default)
		{
			return ReadSetting(key, false, @default);
		}

		private static ConfigurationItem<T> ReadSetting(string key, bool required, T @default)
		{
			var setting = ConfigurationManager.AppSettings[key];

			if (string.IsNullOrEmpty(setting))
			{
				if (required)
				{
					var message = string.Format(InfrastructureResources.ConfigurationItemMissing, key);

					Log.Error(message);

					throw new ApplicationException(message);
				}

				Log.Information(string.Format("[ConfigurationItem] {0} : {1} ({2})", key, @default, InfrastructureResources.ConfigurationItemMissingUsingDefault));

				return new ConfigurationItem<T>(@default);
			}

			if (string.IsNullOrEmpty(setting))
			{
				Log.Information(string.Format("[ConfigurationItem] {0} : {1} ({2})", key, @default, InfrastructureResources.ConfigurationItemMissingUsingDefault));

				return new ConfigurationItem<T>(@default);
			}

			var item = new ConfigurationItem<T>((T)Convert.ChangeType(setting, Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T)));

			Log.Information(string.Format("[ConfigurationItem] {0} : {1}", key, ProtectedValue(key, Convert.ToString(item.GetValue()))));

			return item;
		}

		private static object ProtectedValue(string key, string value)
		{
			var keysValue = ConfigurationManager.AppSettings["ConfigurationItemSensitiveKeys"];
			var useContains = false;

			if (string.IsNullOrEmpty(keysValue))
			{
				useContains = true;

				keysValue = "password;pwd";
			}

			var keys = keysValue.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);

			foreach (string sensitiveKey in keys)
			{
				if (useContains)
				{
					if (key.ToLower().Contains(sensitiveKey))
					{
						return "(sensitive data)";
					}
				}
				else
				{
					if (key.Equals(sensitiveKey, StringComparison.InvariantCultureIgnoreCase))
					{
						return "(sensitive data)";
					}
				}
			}

			return value;
		}
	}
}