using System;
using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
    public class DefaultComponentContainer : IComponentContainer
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

        public IComponentContainer Register(Type serviceType, Type implementationType, Lifestyle lifestyle)
        {
            Guard.AgainstNull(serviceType, "serviceType");
            Guard.AgainstNull(implementationType, "implementationType");

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

        public IComponentContainer Register(Type serviceType, object instance)
        {
            Guard.AgainstNull(serviceType, "serviceType");
            Guard.AgainstNull(instance, "instance");

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
    }
}