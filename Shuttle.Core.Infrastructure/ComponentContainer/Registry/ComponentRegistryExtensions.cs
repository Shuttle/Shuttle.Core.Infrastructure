using System;
using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
    public static class ComponentRegistryExtensions
    {
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
        ///     Registers a new dependency/implementation type pair as a singleton.
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
        ///     Registers a new dependency/implementation type pair.
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
        ///     Registers a new dependency/implementation type pair as a singleton.
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
        ///     Registers a new dependency/implementation type pair.
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
        ///     Registers a singleton instance for the given dependency type.
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
        ///     Registers a new dependency/implementation type pair as a singleton if the dependency has not yet been registered;
        ///     else does nothing.
        /// </summary>
        /// <typeparam name="TDependency">The type of the dependency being registered.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation that should be resolved.</typeparam>
        /// <param name="registry">The registry instance to register the mapping against.</param>
        public static IComponentRegistry AttemptRegister<TDependency, TImplementation>(this IComponentRegistry registry)
            where TDependency : class
            where TImplementation : class, TDependency
        {
            Guard.AgainstNull(registry, "registry");

            if (registry.IsRegistered<TDependency>())
            {
                return registry;
            }

            registry.Register<TDependency, TImplementation>(Lifestyle.Singleton);

            return registry;
        }

        /// <summary>
        ///     Registers a new dependency/implementation type pair if the dependency has not yet been registered; else does
        ///     nothing.
        /// </summary>
        /// <typeparam name="TDependency">The type of the dependency being registered.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation that should be resolved.</typeparam>
        /// <param name="registry">The registry instance to register the mapping against.</param>
        /// <param name="lifestyle">The lifestyle of the component.</param>
        public static IComponentRegistry AttemptRegister<TDependency, TImplementation>(this IComponentRegistry registry,
            Lifestyle lifestyle)
            where TDependency : class
            where TImplementation : TDependency
        {
            Guard.AgainstNull(registry, "registry");

            if (registry.IsRegistered<TDependency>())
            {
                return registry;
            }

            registry.Register(typeof(TDependency), typeof(TImplementation), lifestyle);

            return registry;
        }

        /// <summary>
        ///     Registers a new dependency/implementation type pair as a singleton if the dependency has not yet been registered;
        ///     else does nothing.
        /// </summary>
        /// <typeparam name="TDependencyImplementation">
        ///     The type of the dependency, that is also the implementation, being
        ///     registered.
        /// </typeparam>
        /// <param name="registry">The registry instance to register the mapping against.</param>
        public static IComponentRegistry AttemptRegister<TDependencyImplementation>(this IComponentRegistry registry)
            where TDependencyImplementation : class
        {
            if (registry.IsRegistered<TDependencyImplementation>())
            {
                return registry;
            }

            return registry.Register<TDependencyImplementation>(Lifestyle.Singleton);
        }

        /// <summary>
        ///     Registers a new dependency/implementation type pair if the dependency has not yet been registered; else does
        ///     nothing.
        /// </summary>
        /// <typeparam name="TDependencyImplementation">
        ///     The type of the dependency, that is also the implementation, being
        ///     registered.
        /// </typeparam>
        /// <param name="registry">The registry instance to register the mapping against.</param>
        /// <param name="lifestyle">The lifestyle of the component.</param>
        public static IComponentRegistry AttemptRegister<TDependencyImplementation>(this IComponentRegistry registry,
            Lifestyle lifestyle)
            where TDependencyImplementation : class
        {
            Guard.AgainstNull(registry, "registry");

            if (registry.IsRegistered<TDependencyImplementation>())
            {
                return registry;
            }

            registry.Register(typeof(TDependencyImplementation), typeof(TDependencyImplementation), lifestyle);

            return registry;
        }

        /// <summary>
        ///     Registers a singleton instance for the given dependency type if the dependency has not yet been registered; else
        ///     does nothing.
        /// </summary>
        /// <typeparam name="TDependency">The type of the dependency being registered.</typeparam>
        /// <param name="registry">The registry instance to register the mapping against.</param>
        /// <param name="instance">The singleton instance to be registered.</param>
        public static IComponentRegistry AttemptRegister<TDependency>(this IComponentRegistry registry,
            TDependency instance)
        {
            Guard.AgainstNull(registry, "registry");

            if (registry.IsRegistered<TDependency>())
            {
                return registry;
            }

            registry.Register(typeof(TDependency), instance);

            return registry;
        }

        /// <summary>
        ///     Creates an instance of all types implementing the `IComponentRegistryBootstrap` interface and calls the `Register`
        ///     method.
        /// </summary>
        /// <param name="registry">The `IComponentRegistry` instance to pass register the components in.</param>
        public static void RegistryBoostrap(this IComponentRegistry registry)
        {
            RegistryBoostrap(registry, ComponentRegistrySection.Configuration(), BootstrapSection.Configuration());
        }

        /// <summary>
        ///     Creates an instance of all types implementing the `IComponentRegistryBootstrap` interface and calls the `Register`
        ///     method.
        /// </summary>
        /// <param name="registry">The `IComponentRegistry` instance to pass register the components in.</param>
        /// <param name="registryConfiguration">The `IComponentRegistryConfiguration` instance that contains the registry configuration.</param>
        /// <param name="bootstrapConfiguration">The `IBootstrapConfiguration` instance that contains the bootstrapping configuration.</param>
        public static void RegistryBoostrap(this IComponentRegistry registry,
            IComponentRegistryConfiguration registryConfiguration, IBootstrapConfiguration bootstrapConfiguration)
        {
            Guard.AgainstNull(registry, "registry");

            var completed = new List<Type>();

            var reflectionService = new ReflectionService();

            foreach (var assembly in bootstrapConfiguration.Assemblies)
            {
                foreach (var type in reflectionService.GetTypes<IComponentRegistryBootstrap>(assembly))
                {
                    if (completed.Contains(type))
                    {
                        continue;
                    }

                    type.AssertDefaultConstructor(string.Format(InfrastructureResources.DefaultConstructorRequired,
                        "IComponentRegistryBootstrap", type.FullName));

                    ((IComponentRegistryBootstrap) Activator.CreateInstance(type)).Register(registry);

                    completed.Add(type);
                }
            }

            foreach (var component in registryConfiguration.Components)
            {
                registry.Register(component.DependencyType, component.ImplementationType, component.Lifestyle);
            }

            foreach (var collection in registryConfiguration.Collections)
            {
                registry.RegisterCollection(collection.DependencyType, collection.ImplementationTypes,
                    collection.Lifestyle);
            }
        }
    }
}