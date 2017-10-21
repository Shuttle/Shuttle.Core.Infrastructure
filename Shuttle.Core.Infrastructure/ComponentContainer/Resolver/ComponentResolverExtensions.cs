using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Shuttle.Core.Infrastructure
{
    public static class ComponentResolverExtensions
    {
        /// <summary>
        ///     Resolves the requested service type.  If the service type cannot be resolved an exception is thrown.
        /// </summary>
        /// <typeparam name="T">The type of the service that should be resolved.</typeparam>
        /// <param name="resolver">The resolver instance that contains the registered service.</param>
        /// <returns>An instance of the type implementing the requested service type.</returns>
        public static T Resolve<T>(this IComponentResolver resolver) where T : class
        {
            Guard.AgainstNull(resolver, "resolver");

            return (T)resolver.Resolve(typeof(T));
        }

        /// <summary>
        ///     Attempts to resolve the requested service type.  If the service type cannot be resolved null is returned.
        /// </summary>
        /// <typeparam name="T">The type of the service that should be resolved.</typeparam>
        /// <param name="resolver">The resolver instance that contains the registered service.</param>
        /// <returns>An instance of the type implementing the requested service type if it can be resolved; else null.</returns>
        public static T AttemptResolve<T>(this IComponentResolver resolver) where T : class
        {
            return (T)AttemptResolve(resolver, typeof(T));
        }

        /// <summary>
        ///     Attempts to resolve the requested service type.  If the service type cannot be resolved null is returned.
        /// </summary>
        /// <param name="resolver">The resolver instance that contains the registered service.</param>
        /// <param name="dependencyType">>The type of the service that should be resolved.</param>
        /// <returns>An instance of the type implementing the requested service type if it can be resolved; else null.</returns>
        public static object AttemptResolve(this IComponentResolver resolver, Type dependencyType)
        {
            Guard.AgainstNull(resolver, "resolver");
            Guard.AgainstNull(dependencyType, "dependencyType");

            try
            {
                return resolver.Resolve(dependencyType);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Resolves all the given types.  These may be types that will not necessarily be injected into another class but that
        ///     may require other instances from the resolver.
        /// </summary>
        /// <param name="resolver">The resolver instance that contains the registered services.</param>
        /// <param name="dependencyTypes">The list of service types that need to be resolved.</param>
        public static IEnumerable<object> Resolve(this IComponentResolver resolver, IEnumerable<Type> dependencyTypes)
        {
            Guard.AgainstNull(resolver, "resolver");

            var result = new List<object>();
            var types = dependencyTypes as IList<Type> ?? dependencyTypes.ToList();

            if (!types.Any())
            {
                return result;
            }

            result.AddRange(types.Select(resolver.Resolve));

            return result;
        }

        /// <summary>
        ///     Resolves all registered instance of the requested service type.
        /// </summary>
        /// <typeparam name="T">The type of the services that should be resolved.</typeparam>
        /// <param name="resolver">The resolver instance that contains the registered service.</param>
        /// <returns>All instances of the types implementing the requested service type.</returns>
        public static IEnumerable<T> ResolveAll<T>(this IComponentResolver resolver) where T : class
        {
            Guard.AgainstNull(resolver, "resolver");

            return resolver.ResolveAll(typeof(T)).Cast<T>().ToList();
        }


        /// <summary>
        ///     Creates an instance of all types implementing the `IComponentResolverBootstrap` interface and calls the `Resolve`
        ///     method.
        /// </summary>
        /// <param name="resolver">The `IComponentResolver` instance to resolve dependencies from.</param>
        public static void ResolverBoostrap(this IComponentResolver resolver)
        {
            ResolverBoostrap(resolver, ComponentResolverSection.Configuration(), BootstrapSection.Configuration());
        }

        /// <summary>
        ///     Creates an instance of all types implementing the `IComponentResolverBootstrap` interface and calls the `Resolve`
        ///     method.
        /// </summary>
        /// <param name="resolver">The `IComponentResolver` instance to resolve dependencies from.</param>
        /// <param name="resolverConfiguration">The `IComponentResolverConfiguration` instance that contains the registry configuration.</param>
        /// <param name="bootstrapConfiguration">The `IBootstrapConfiguration` instance that contains the bootstrapping configuration.</param>
        public static void ResolverBoostrap(IComponentResolver resolver, IComponentResolverConfiguration resolverConfiguration, IBootstrapConfiguration bootstrapConfiguration)
        {
            Guard.AgainstNull(resolver, "resolver");
            Guard.AgainstNull(resolverConfiguration, nameof(resolverConfiguration));

            var reflectionService = new ReflectionService();

            foreach (var assembly in bootstrapConfiguration.Assemblies)
            {
                foreach (var type in reflectionService.GetTypes<IComponentResolverBootstrap>(assembly))
                {
                    type.AssertDefaultConstructor(string.Format(InfrastructureResources.DefaultConstructorRequired,
                        "IComponentResolverBootstrap", type.FullName));

                    ((IComponentResolverBootstrap)Activator.CreateInstance(type)).Resolve(resolver);
                }
            }

            foreach (var component in resolverConfiguration.Components)
            {
                resolver.Resolve(component.DependencyType);
            }
        }
    }
}