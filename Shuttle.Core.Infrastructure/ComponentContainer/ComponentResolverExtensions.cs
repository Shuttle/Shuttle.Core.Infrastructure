using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            Guard.AgainstNull(resolver, "resolver");

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
            Guard.AgainstNull(resolver, "resolver");
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

        /// <summary>
        /// Instantiates (resolves) all the given types.  These may be types that will not necessarily be injected into another class but that may require other instances from the resolver.
        /// </summary>
        /// <param name="resolver">The resolver instance that contains the registered services.</param>
        /// <param name="serviceTypes">The list of service types that need to be resolved.</param>
        public static IEnumerable<object> Instantiate(this IComponentResolver resolver, IEnumerable<Type> serviceTypes)
        {
            Guard.AgainstNull(resolver, "resolver");

            var result = new List<object>();
            var types = serviceTypes as IList<Type> ?? serviceTypes.ToList();

            if (serviceTypes == null || !types.Any())
            {
                return result;
            }

            result.AddRange(types.Select(resolver.Resolve));

            return result;
        }
    }
}