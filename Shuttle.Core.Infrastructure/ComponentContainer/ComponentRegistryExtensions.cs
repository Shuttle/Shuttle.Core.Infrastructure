using System;

namespace Shuttle.Core.Infrastructure
{
    public static class ComponentRegistryExtensions
    {
        /// <summary>
        /// Register a new service/implementation type pair as a singleton.
        /// </summary>
        /// <typeparam name="TService">The type of the service being registered.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation that should be resolved.</typeparam>
        /// <param name="registry">The registry instance to register the mapping against.</param>
        public static void Register<TService, TImplementation>(this IComponentRegistry registry)
            where TService : class
            where TImplementation : class
        {
            Guard.AgainstNull(registry, "registry");

            registry.Register<TService, TImplementation>(Lifestyle.Singleton);
        }

        /// <summary>
        /// Register a new service/implementation type pair as a singleton.
        /// </summary>
        /// <typeparam name="TService">The type of the service being registered.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation that should be resolved.</typeparam>
        /// <param name="registry">The registry instance to register the mapping against.</param>
        /// <param name="lifestyle">The lifestyle of the component.</param>
        public static void Register<TService, TImplementation>(this IComponentRegistry registry, Lifestyle lifestyle)
            where TService : class
            where TImplementation : class
        {
            Guard.AgainstNull(registry, "registry");

            registry.Register(typeof(TService), typeof(TImplementation), lifestyle);
        }

        /// <summary>
        /// Register a singleton instance for the given service type.
        /// </summary>
        /// <typeparam name="TService">The type of the service being registered.</typeparam>
        /// <param name="registry">The registry instance to register the mapping against.</param>
        /// <param name="instance">The singleton instance to be registered.</param>
        public static void Register<TService>(this IComponentRegistry registry, TService instance)
        {
            Guard.AgainstNull(registry, "registry");

            registry.Register(typeof(TService), instance);
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

            foreach (ComponentElement componentElement in section.Components)
            {
                var serviceType = Type.GetType(componentElement.ServiceType, true);
                var implementationType = !string.IsNullOrEmpty(componentElement.ImplementationType) ? Type.GetType(componentElement.ImplementationType) : serviceType;

                registry.Register(serviceType, implementationType, componentElement.Lifestyle);
            }
        }
    }
}