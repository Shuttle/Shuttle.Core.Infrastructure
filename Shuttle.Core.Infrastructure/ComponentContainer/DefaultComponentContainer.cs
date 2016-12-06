using System;
using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
    public class DefaultComponentContainer : IComponentRegistry, IComponentResolver, IDisposable
    {
        private static readonly object _lock = new object();
        private readonly Dictionary<Type, ImplementationDefinition> _map = new Dictionary<Type, ImplementationDefinition>();

        public object Resolve(Type serviceType)
        {
            Guard.AgainstNull(serviceType, "serviceType");

            lock (_lock)
            {
                if (_map.ContainsKey(serviceType))
                {
                    return _map[serviceType].Get(this);
                }

                throw new TypeResolutionException(string.Format(InfrastructureResources.TypeNotRegisteredException, serviceType.FullName));
            }
        }

        public T Resolve<T>() where T : class
        {
            return Resolve(typeof(T)) as T;
        }

        public IComponentResolver Register(Type serviceType, Type implementationType, Lifestyle lifestyle)
        {
            Guard.AgainstNull(serviceType, "serviceType");
            Guard.AgainstNull(implementationType, "implementationType");

            if (!serviceType.IsAssignableFrom(implementationType))
            {
                  throw new TypeRegistrationException(string.Format(InfrastructureResources.UnassignableTypeRegistrationException, serviceType.FullName, implementationType.FullName));
            }

            lock (_lock)
            {
                if (!_map.ContainsKey(serviceType))
                {
                    _map.Add(serviceType, new ImplementationDefinition(implementationType, lifestyle));
                }
                else
                {
                    throw new TypeRegistrationException(
                        string.Format(InfrastructureResources.DuplicateTypeRegistrationException,
                            _map[serviceType].Type.FullName, serviceType.FullName, implementationType.FullName));
                }
            }

            return this;
        }

        public IComponentResolver Register(Type serviceType, object instance)
        {
            Guard.AgainstNull(serviceType, "serviceType");
            Guard.AgainstNull(instance, "instance");

            if (!serviceType.IsInstanceOfType(instance))
            {
                throw new TypeRegistrationException(string.Format(InfrastructureResources.UnassignableTypeRegistrationException, serviceType.FullName, instance.GetType().FullName));
            }

            lock (_lock)
            {
                if (!_map.ContainsKey(serviceType))
                {
                    _map.Add(serviceType, new ImplementationDefinition(instance));
                }
                else
                {
                    throw new TypeRegistrationException(
                        string.Format(InfrastructureResources.DuplicateTypeRegistrationException,
                            _map[serviceType].Type.FullName, serviceType.FullName, instance.GetType().FullName));
                }
            }

            return this;
        }

        public bool IsRegistered(Type serviceType)
        {
            Guard.AgainstNull(serviceType, "serviceType");

            lock (_lock)
            {
                return _map.ContainsKey(serviceType);
            }
        }

        public void Dispose()
        {
            lock(_lock)
            {
                foreach (var implementationDefinition in _map.Values)
                {
                    implementationDefinition.Dispose();
                }
            }
        }
    }
}