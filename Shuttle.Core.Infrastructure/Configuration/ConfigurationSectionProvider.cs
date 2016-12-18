using System;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
	public class ConfigurationSectionProvider
	{
		public static T Open<T>(string name) where T : class
		{
			return ConfigurationManager.GetSection(name) as T;
		}

		public static T Open<T>(string group, string name) where T : class
		{
			var key = string.Format("{0}/{1}", group, name);
			return (ConfigurationManager.GetSection(key) ?? ConfigurationManager.GetSection(name)) as T;
		}

		public static T OpenFile<T>(string name, string file) where T : class
		{
			return ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(file)).GetSection(name) as T;
		}

		public static T OpenFile<T>(string group, string name, string file) where T : class
		{
			var configuration = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(file));

			var sectionGroup = configuration.GetSectionGroup(group);

			return (sectionGroup == null || sectionGroup.Sections[name] == null ? configuration.GetSection(name) : sectionGroup.Sections[name]) as T;
		}
	}
}