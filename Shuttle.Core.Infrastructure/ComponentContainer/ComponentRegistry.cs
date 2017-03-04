using System;
using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
	public abstract class ComponentRegistry : IComponentRegistry
	{
		private readonly List<Type> _registeredTypes = new List<Type>();

		public bool IsRegistered(Type type)
		{
			Guard.AgainstNull(type, "type");

			return _registeredTypes.Contains(type);
		}

		private void DependencyInvariant(Type type)
		{
			if (IsRegistered(type))
			{
				throw new TypeRegistrationException(string.Format(InfrastructureResources.DuplicateTypeRegistrationException, type.FullName));
			}
		}

		public virtual IComponentRegistry Register(Type dependencyType, Type implementationType, Lifestyle lifestyle)
		{
			Guard.AgainstNull(dependencyType, "dependencyType");
			Guard.AgainstNull(implementationType, "implementationType");

			DependencyInvariant(dependencyType);

			_registeredTypes.Add(dependencyType);

			return this;
		}

		public virtual IComponentRegistry RegisterCollection(Type dependencyType, IEnumerable<Type> implementationTypes,
			Lifestyle lifestyle)
		{
			Guard.AgainstNull(dependencyType, "dependencyType");
			Guard.AgainstNull(implementationTypes, "implementationTypes");

			DependencyInvariant(dependencyType);

			_registeredTypes.Add(dependencyType);

			return this;
		}

		public virtual IComponentRegistry Register(Type dependencyType, object instance)
		{
			Guard.AgainstNull(dependencyType, "dependencyType");
			Guard.AgainstNull(instance, "instance");

			DependencyInvariant(dependencyType);

			_registeredTypes.Add(dependencyType);

			return this;
		}
	}
}