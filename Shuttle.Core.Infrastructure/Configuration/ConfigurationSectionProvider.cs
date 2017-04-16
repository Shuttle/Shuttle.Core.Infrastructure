using System;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
	public class ConfigurationSectionProvider
	{
		public static T Open<T>(string name) where T : ConfigurationSection
        {
			return (T)ConfigurationManager.GetSection(name);
		}

		public static T Open<T>(string group, string name) where T : ConfigurationSection
        {
			var key = string.Format("{0}/{1}", group, name);

		    return (T)(ConfigurationManager.GetSection(key) ?? ConfigurationManager.GetSection(name));
		}

		public static T OpenFile<T>(string name, string file) where T : ConfigurationSection
		{
			return (T)ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(file)).GetSection(name);
		}

		public static T OpenFile<T>(string group, string name, string file) where T : ConfigurationSection
        {
			var configuration = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(file));

			var sectionGroup = configuration.GetSectionGroup(group);

			return (T)(sectionGroup == null || sectionGroup.Sections[name] == null ? configuration.GetSection(name) : sectionGroup.Sections[name]);
		}
	}
}