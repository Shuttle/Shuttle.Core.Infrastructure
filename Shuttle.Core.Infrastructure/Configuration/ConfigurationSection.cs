using System;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
	public class ConfigurationSection
	{
		public static T Open<T>(string name) where T : class
		{
			var section = ConfigurationManager.GetSection(name) as T;

			if (section == null)
			{
				throw new ConfigurationErrorsException(string.Format(InfrastructureResources.OpenSectionException, name, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, typeof(T).FullName));
			}

			return section;
		}

		public static T Open<T>(string group, string name) where T : class
		{
			var key = string.Format("{0}/{1}", group, name);
			var section = (ConfigurationManager.GetSection(key) ?? ConfigurationManager.GetSection(name)) as T;

			if (section == null)
			{
				throw new ConfigurationErrorsException(string.Format(InfrastructureResources.OpenSectionException, key, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, typeof(T).FullName));
			}

			return section;
		}

		public static T OpenFile<T>(string name, string file) where T : class
		{
			var configuration = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(file));

			var section = configuration.GetSection(name) as T;

			if (section == null)
			{
				throw new ConfigurationErrorsException(string.Format(InfrastructureResources.OpenSectionException, name, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, typeof(T).FullName));
			}

			return section;
		}

		public static T OpenFile<T>(string group, string name, string file) where T : class
		{
			var configuration = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(file));

			var sectionGroup = configuration.GetSectionGroup(group);

			var section = sectionGroup == null ? configuration.GetSection(name) as T : sectionGroup.Sections[name] as T;

			if (section == null)
			{
				throw new ConfigurationErrorsException(string.Format(InfrastructureResources.OpenSectionException, string.Format("{0}/{1}", group, name), file, typeof(T).FullName));
			}

			return section;
		}
	}
}