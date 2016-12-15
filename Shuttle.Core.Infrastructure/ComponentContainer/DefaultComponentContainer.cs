using System;
using System.Collections;
using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
    public class DefaultComponentContainer : IComponentRegistry, IComponentResolver, IDisposable
    {
        private static readonly object _lock = new object();
        private readonly Dictionary<Type, Dictionary<string, ImplementationDefinition>> _map = new Dictionary<Type, Dictionary<string, ImplementationDefinition>>();

        public object Resolve(Type serviceType)
        {
            Guard.AgainstNull(serviceType, "serviceType");

            lock (_lock)
            {
                if (_map.ContainsKey(serviceType))
                {
                    var implementationDefinitions = _map[serviceType];

                    if (!implementationDefinitions.ContainsKey(string.Empty))
                    {
                        throw new TypeResolutionException(string.Format(InfrastructureResources.ResolveException, serviceType.FullName));
                    }

                    return implementationDefinitions[string.Empty].Get(this);
                }

                throw new TypeResolutionException(string.Format(InfrastructureResources.TypeNotRegisteredException, serviceType.FullName));
            }
        }

        public object Resolve(string name, Type serviceType)
        {
            Guard.AgainstNullOrEmptyString(name, "name");
            Guard.AgainstNull(serviceType, "serviceType");

            lock (_lock)
            {
                if (_map.ContainsKey(serviceType))
                {
                    var implementationDefinitions = _map[serviceType];

                    if (!implementationDefinitions.ContainsKey(name))
                    {
                        throw new TypeResolutionException(string.Format(InfrastructureResources.ResolveNameException, name, serviceType.FullName));
                    }

                    return implementationDefinitions[name].Get(this);
                }

                throw new TypeResolutionException(string.Format(InfrastructureResources.TypeNotRegisteredException, serviceType.FullName));
            }
        }

        public IEnumerable ResolveAll(Type serviceType)
        {
            Guard.AgainstNull(serviceType, "serviceType");

            var result = new List<object>();

            lock (_lock)
            {
                if (_map.ContainsKey(serviceType))
                {
                    var implementationDefinitions = _map[serviceType];

                    foreach (var implementationDefinition in implementationDefinitions)
                    {
                        result.Add(implementationDefinition.Value.Get(this));
                    }
                }
            }

            return result;
        }

        public T Resolve<T>() where T : class
        {
            return Resolve(typeof(T)) as T;
        }

        public IComponentRegistry Register(Type serviceType, Type implementationType, Lifestyle lifestyle)
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
                    _map.Add(serviceType, new Dictionary<string, ImplementationDefinition>
                    {
                        {string.Empty, new ImplementationDefinition(implementationType, lifestyle)}
                    });
                }
                else
                {
                    throw new TypeRegistrationException(string.Format(InfrastructureResources.DuplicateTypeRegistrationException, serviceType.FullName));
                }
            }

            return this;
        }

        public IComponentRegistry Register(string name, Type serviceType, Type implementationType, Lifestyle lifestyle)
        {
            Guard.AgainstNull(serviceType, "serviceType");
            Guard.AgainstNull(implementationType, "implementationType");

            if (!serviceType.IsAssignableFrom(implementationType))
            {
                throw new TypeRegistrationException(string.Format(InfrastructureResources.UnassignableTypeRegistrationException, serviceType.FullName, implementationType.FullName));
            }

            lock (_lock)
            {
                Dictionary<string, ImplementationDefinition> implementationDefinitions;

                if (!_map.ContainsKey(serviceType))
                {
                    implementationDefinitions = new Dictionary<string, ImplementationDefinition>();

                    _map.Add(serviceType, implementationDefinitions);
                }
                else
                {
                    implementationDefinitions = _map[serviceType];

                    if (implementationDefinitions.ContainsKey(string.Empty))
                    {
                        throw new TypeRegistrationException(string.Format(InfrastructureResources.InvalidNamedTypeRegistrationException, serviceType.FullName));
                    }

                    if (implementationDefinitions.ContainsKey(name))
                    {
                        throw new TypeRegistrationException(string.Format(InfrastructureResources.DuplicateNamedTypeRegistrationException, serviceType.FullName));
                    }
                }

                implementationDefinitions.Add(name, new ImplementationDefinition(implementationType, lifestyle));
            }

            return this;
        }

        public IComponentRegistry Register(Type serviceType, object instance)
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
                    _map.Add(serviceType, new Dictionary<string, ImplementationDefinition>
                    {
                        {string.Empty, new ImplementationDefinition(instance)}
                    });
                }
                else
                {
                    throw new TypeRegistrationException(string.Format(InfrastructureResources.DuplicateTypeRegistrationException, serviceType.FullName));
                }
            }

            return this;
        }

        public void Dispose()
        {
            lock (_lock)
            {
                foreach (var dictionary in _map.Values)
                {
                    foreach (var implementationDefinition in dictionary.Values)
                    {
                        implementationDefinition.Dispose();
                    }
                }
            }
        }
    }
}