using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shuttle.Core.Infrastructure
{
	public static class ComponentRegistryExtensions
	{
		private static readonly List<Type> EmptyTypes = new List<Type>();

		/// <summary>
		///     Determines whether the component registry has a dependency of the given type registered.
		/// </summary>
		/// <typeparam name="TDependency">The type of the dependency that is being checked.</typeparam>
		/// <param name="registry">The registry instance to register the mapping against.</param>
		/// <returns>Returns `true` if the dependency type is registered; else `false`.</returns>
		public static bool IsRegistered<TDependency>(this IComponentRegistry registry)
		{
			Guard.AgainstNull(registry, "registry");

			return registry.IsRegistered(typeof(TDependency));
		}

		/// <summary>
		///     Register a new dependency/implementation type pair as a singleton.
		/// </summary>
		/// <typeparam name="TDependency">The type of the dependency being registered.</typeparam>
		/// <typeparam name="TImplementation">The type of the implementation that should be resolved.</typeparam>
		/// <param name="registry">The registry instance to register the mapping against.</param>
		public static IComponentRegistry Register<TDependency, TImplementation>(this IComponentRegistry registry)
			where TDependency : class
			where TImplementation : class, TDependency
		{
			Guard.AgainstNull(registry, "registry");

			registry.Register<TDependency, TImplementation>(Lifestyle.Singleton);

			return registry;
		}

		/// <summary>
		///     Register a new dependency/implementation type pair.
		/// </summary>
		/// <typeparam name="TDependency">The type of the dependency being registered.</typeparam>
		/// <typeparam name="TImplementation">The type of the implementation that should be resolved.</typeparam>
		/// <param name="registry">The registry instance to register the mapping against.</param>
		/// <param name="lifestyle">The lifestyle of the component.</param>
		public static IComponentRegistry Register<TDependency, TImplementation>(this IComponentRegistry registry,
			Lifestyle lifestyle)
			where TDependency : class
			where TImplementation : TDependency
		{
			Guard.AgainstNull(registry, "registry");

			registry.Register(typeof(TDependency), typeof(TImplementation), lifestyle);

			return registry;
		}

		/// <summary>
		///     Register a new dependency/implementation type pair as a singleton.
		/// </summary>
		/// <typeparam name="TDependencyImplementation">
		///     The type of the dependency, that is also the implementation, being
		///     registered.
		/// </typeparam>
		/// <param name="registry">The registry instance to register the mapping against.</param>
		public static IComponentRegistry Register<TDependencyImplementation>(this IComponentRegistry registry)
			where TDependencyImplementation : class
		{
			return registry.Register<TDependencyImplementation>(Lifestyle.Singleton);
		}

		/// <summary>
		///     Register a new dependency/implementation type pair.
		/// </summary>
		/// <typeparam name="TDependencyImplementation">
		///     The type of the dependency, that is also the implementation, being
		///     registered.
		/// </typeparam>
		/// <param name="registry">The registry instance to register the mapping against.</param>
		/// <param name="lifestyle">The lifestyle of the component.</param>
		public static IComponentRegistry Register<TDependencyImplementation>(this IComponentRegistry registry,
			Lifestyle lifestyle)
			where TDependencyImplementation : class
		{
			Guard.AgainstNull(registry, "registry");

			registry.Register(typeof(TDependencyImplementation), typeof(TDependencyImplementation), lifestyle);

			return registry;
		}

		/// <summary>
		///     Register a singleton instance for the given dependency type.
		/// </summary>
		/// <typeparam name="TDependency">The type of the dependency being registered.</typeparam>
		/// <param name="registry">The registry instance to register the mapping against.</param>
		/// <param name="instance">The singleton instance to be registered.</param>
		public static IComponentRegistry Register<TDependency>(this IComponentRegistry registry, TDependency instance)
		{
			Guard.AgainstNull(registry, "registry");

			registry.Register(typeof(TDependency), instance);

			return registry;
		}

		/// <summary>
		///     Registers all components from the 'componentRegistry' section in the application configuration file.
		/// </summary>
		/// <param name="registry">The registry instance to register the configuration section against.</param>
		public static IComponentRegistry RegisterSection(this IComponentRegistry registry)
		{
			Guard.AgainstNull(registry, "registry");

			var section = ConfigurationSectionProvider.OpenFile<ComponentRegistrySection>("shuttle", "componentRegistry");

			if (section == null)
			{
				return registry;
			}

			foreach (ComponentRegistryComponentElement componentElement in section.Components)
			{
				var dependencyType = Type.GetType(componentElement.DependencyType, true);
				var implementationType = !string.IsNullOrEmpty(componentElement.ImplementationType)
					? Type.GetType(componentElement.ImplementationType)
					: dependencyType;

				registry.Register(dependencyType, implementationType, componentElement.Lifestyle);
			}

			return registry;
		}

		/// <summary>
		///     Registers all types in the given assembly that implement interface `IPipelineObserver`.
		/// </summary>
		/// <param name="registry">The `IComponentRegistry` instance to register the types in.</param>
		/// <param name="assembly">The `Assembly` instance that should be scanned.</param>
		public static IComponentRegistry RegisterObservers(this IComponentRegistry registry, Assembly assembly)
		{
			return RegisterObservers(registry, assembly, null);
		}

		/// <summary>
		///     Registers all types in the given assembly that implement interface `IPipelineObserver`.
		/// </summary>
		/// <param name="registry">The `IComponentRegistry` instance to register the types in.</param>
		/// <param name="assembly">The `Assembly` instance that should be scanned.</param>
		/// <param name="dontRegisterTypes">
		///     A list of types that implement the `IPipelineObserver` that have to be ignored (not
		///     registered).
		/// </param>
		public static IComponentRegistry RegisterObservers(this IComponentRegistry registry, Assembly assembly,
			IEnumerable<Type> dontRegisterTypes)
		{
			Guard.AgainstNull(registry, "registry");
			Guard.AgainstNull(assembly, "assembly");

			var reflectionService = new ReflectionService();

			var typesToIgnore = dontRegisterTypes == null ? EmptyTypes : dontRegisterTypes.ToList();

			foreach (var type in reflectionService.GetTypes<IPipelineObserver>(assembly))
			{
				if (type.IsInterface || registry.IsRegistered(type) || dontRegisterTypes != null && typesToIgnore.Contains(type))
				{
					continue;
				}

				registry.Register(type, type, Lifestyle.Singleton);
			}

			return registry;
		}
	}
}