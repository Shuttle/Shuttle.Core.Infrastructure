using System;

namespace Shuttle.Core.Infrastructure
{
    public static class ComponentResolverExtensions
    {
        /// <summary>
        /// Resolves the requested service type.  If the service type cannot be resolved an exception is thrown.
        /// </summary>
        /// <typeparam name="T">The type of the service that should be resolved.</typeparam>
        /// <param name="resolver">The resolver instance that contains the registered service.</param>
        /// <returns>An instance of the type implementing the requested service type.</returns>
        public static T Resolve<T>(this IComponentResolver resolver) where T : class
        {
            Guard.AgainstNull(resolver, "registry");

            return (T)resolver.Resolve(typeof(T));
        }

        /// <summary>
        /// Attempts to resolve the requested service type.  If the service type cannot be resolved null is returned.
        /// </summary>
        /// <typeparam name="T">The type of the service that should be resolved.</typeparam>
        /// <param name="resolver">The resolver instance that contains the registered service.</param>
        /// <returns>An instance of the type implementing the requested service type if it can be resolved; else null.</returns>
        public static T AttemptResolve<T>(this IComponentResolver resolver) where T : class
        {
            return (T)AttemptResolve(resolver, typeof(T));
        }

        /// <summary>
        /// Attempts to resolve the requested service type.  If the service type cannot be resolved null is returned.
        /// </summary>
        /// <param name="resolver">The resolver instance that contains the registered service.</param>
        /// <param name="serviceType">>The type of the service that should be resolved.</param>
        /// <returns>An instance of the type implementing the requested service type if it can be resolved; else null.</returns>
        public static object AttemptResolve(this IComponentResolver resolver, Type serviceType)
        {
            Guard.AgainstNull(resolver, "registry");
            Guard.AgainstNull(serviceType, "serviceType");

            try
            {
                return resolver.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }
    }
}