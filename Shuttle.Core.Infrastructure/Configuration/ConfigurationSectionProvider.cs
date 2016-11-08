using System;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
	public class ConfigurationSectionProvider
	{
		public static T Open<T>(string name) where T : class
		{
			var section = ConfigurationManager.GetSection(name) as T;

			if (section == null)
			{
				Log.Warning(string.Format(InfrastructureResources.SectionNotFound, name, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, typeof(T).FullName));
			}

			return section;
		}

		public static T Open<T>(string group, string name) where T : class
		{
			var key = string.Format("{0}/{1}", group, name);
			var section = (ConfigurationManager.GetSection(key) ?? ConfigurationManager.GetSection(name)) as T;

			if (section == null)
			{
				Log.Warning(string.Format(InfrastructureResources.SectionNotFound, key, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, typeof(T).FullName));
			}

			return section;
		}

		public static T OpenFile<T>(string name, string file) where T : class
		{
			var configuration = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(file));

			var section = configuration.GetSection(name) as T;

			if (section == null)
			{
				Log.Warning(string.Format(InfrastructureResources.SectionNotFound, name, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, typeof(T).FullName));
			}

			return section;
		}

		public static T OpenFile<T>(string group, string name, string file) where T : class
		{
			var configuration = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(file));

			var sectionGroup = configuration.GetSectionGroup(group);

			var section = (sectionGroup == null || sectionGroup.Sections[name] == null ? configuration.GetSection(name) : sectionGroup.Sections[name]) as T;

			if (section == null)
			{
				Log.Warning(string.Format(InfrastructureResources.SectionNotFound, string.Format("{0}/{1}", group, name), file, typeof(T).FullName));
			}

			return section;
		}
	}
}