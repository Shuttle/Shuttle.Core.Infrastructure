using System;

namespace Shuttle.Core.Infrastructure
{
    public static class ComponentContainerExtensions
    {
        /// <summary>
        /// Register a new service/implementation type pair as a singleton.
        /// </summary>
        /// <typeparam name="TService">The type of the service being registered.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation that should be resolved.</typeparam>
        /// <param name="container">The container instance to register the mapping against.</param>
        public static void Register<TService, TImplementation>(this IComponentContainer container)
            where TService : class
            where TImplementation : class
        {
            Guard.AgainstNull(container, "container");

            container.Register<TService, TImplementation>(Lifestyle.Singleton);
        }

        /// <summary>
        /// Register a new service/implementation type pair as a singleton.
        /// </summary>
        /// <typeparam name="TService">The type of the service being registered.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation that should be resolved.</typeparam>
        /// <param name="container">The container instance to register the mapping against.</param>
        /// <param name="lifestyle">The lifestyle of the component.</param>
        public static void Register<TService, TImplementation>(this IComponentContainer container, Lifestyle lifestyle)
            where TService : class
            where TImplementation : class
        {
            Guard.AgainstNull(container, "container");

            container.Register(typeof(TService), typeof(TImplementation), lifestyle);
        }

        /// <summary>
        /// Register a singleton instance for the given service type.
        /// </summary>
        /// <typeparam name="TService">The type of the service being registered.</typeparam>
        /// <param name="container">The container instance to register the mapping against.</param>
        /// <param name="instance">The singleton instance to be registered.</param>
        public static void Register<TService>(this IComponentContainer container, TService instance)
        {
            Guard.AgainstNull(container, "container");

            container.Register(typeof(TService), instance);
        }

        /// <summary>
        /// Resolves the requested service type.  If the service type cannot be resolved an exception is thrown.
        /// </summary>
        /// <typeparam name="T">The type of the service that should be resolved.</typeparam>
        /// <param name="container">The container instance that contains the registered service.</param>
        /// <returns>An instance of the type implementing the requested service type.</returns>
        public static T Resolve<T>(this IComponentContainer container) where T : class
        {
            Guard.AgainstNull(container, "container");

            return (T)container.Resolve(typeof(T));
        }

        /// <summary>
        /// Attempts to resolve the requested service type.  If the service type cannot be resolved null is returned.
        /// </summary>
        /// <typeparam name="T">The type of the service that should be resolved.</typeparam>
        /// <param name="container">The container instance that contains the registered service.</param>
        /// <returns>An instance of the type implementing the requested service type if it can be resolved; else null.</returns>
        public static T AttemptResolve<T>(this IComponentContainer container) where T : class
        {
            return (T)AttemptResolve(container, typeof(T));
        }

        /// <summary>
        /// Attempts to resolve the requested service type.  If the service type cannot be resolved null is returned.
        /// </summary>
        /// <param name="container">The container instance that contains the registered service.</param>
        /// <param name="serviceType">>The type of the service that should be resolved.</param>
        /// <returns>An instance of the type implementing the requested service type if it can be resolved; else null.</returns>
        public static object AttemptResolve(this IComponentContainer container, Type serviceType)
        {
            Guard.AgainstNull(container, "container");
            Guard.AgainstNull(serviceType, "serviceType");

            try
            {
                return container.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }
    }
}