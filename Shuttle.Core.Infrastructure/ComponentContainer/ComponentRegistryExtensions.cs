using System;

namespace Shuttle.Core.Infrastructure
{
    public static class ComponentRegistryExtensions
    {
        /// <summary>
        /// Register a new service/implementation type pair as a singleton.
        /// </summary>
        /// <typeparam name="TDependency">The type of the service being registered.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation that should be resolved.</typeparam>
        /// <param name="registry">The registry instance to register the mapping against.</param>
        public static void Register<TDependency, TImplementation>(this IComponentRegistry registry)
            where TDependency : class
            where TImplementation : class, TDependency
        {
            Guard.AgainstNull(registry, "registry");

            registry.Register<TDependency, TImplementation>(Lifestyle.Singleton);
        }

        /// <summary>
        /// Register a new service/implementation type pair as a singleton.
        /// </summary>
        /// <typeparam name="TDependency">The type of the dependency being registered.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation that should be resolved.</typeparam>
        /// <param name="registry">The registry instance to register the mapping against.</param>
        /// <param name="lifestyle">The lifestyle of the component.</param>
        public static void Register<TDependency, TImplementation>(this IComponentRegistry registry, Lifestyle lifestyle)
            where TDependency : class
            where TImplementation : TDependency
        {
            Guard.AgainstNull(registry, "registry");

            registry.Register(typeof(TDependency), typeof(TImplementation), lifestyle);
        }

        /// <summary>
        /// Register a singleton instance for the given service type.
        /// </summary>
        /// <typeparam name="TDependency">The type of the service being registered.</typeparam>
        /// <param name="registry">The registry instance to register the mapping against.</param>
        /// <param name="instance">The singleton instance to be registered.</param>
        public static void Register<TDependency>(this IComponentRegistry registry, TDependency instance)
        {
            Guard.AgainstNull(registry, "registry");

            registry.Register(typeof(TDependency), instance);
        }

        /// <summary>
        /// Registers all components from the 'componentRegistry' section in the application configuration file.
        /// </summary>
        /// <param name="registry">The registry instance to register the configuration section against.</param>
        public static void RegisterSection(this IComponentRegistry registry)
        {
            Guard.AgainstNull(registry, "registry");

            var section = ConfigurationSectionProvider.OpenFile<ComponentRegistrySection>("shuttle", "componentRegistry");

            if (section == null)
            {
                return;
            }

            foreach (ComponentRegistryComponentElement componentElement in section.Components)
            {
                var dependencyType = Type.GetType(componentElement.DependencyType, true);
                var implementationType = !string.IsNullOrEmpty(componentElement.ImplementationType) ? Type.GetType(componentElement.ImplementationType) : dependencyType;

                registry.Register(dependencyType, implementationType, componentElement.Lifestyle);
            }
        }
    }
}