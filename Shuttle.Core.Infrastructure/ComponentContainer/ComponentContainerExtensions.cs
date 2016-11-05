using System;

namespace Shuttle.Core.Infrastructure
{
    public static class ComponentContainerExtensions
    {
        public static T AttemptResolve<T>(this IComponentContainer container) where T : class
        {
            return (T) AttemptResolve(container, typeof (T));
        }

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