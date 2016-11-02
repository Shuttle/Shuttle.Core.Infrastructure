using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shuttle.Core.Infrastructure
{
    public class ImplementationDefinition
    {
        private readonly Type _lifestyleType = typeof(Lifestyle);
        private readonly object _lock = new object();
        private object _instance;

        [ThreadStatic]
        private static object _threadInstance;

        private readonly ConstructorInfo _constructor;
        private readonly ParameterInfo[] _constructorParameters;

        public Type Type { get; private set; }
        public Lifestyle Lifestyle { get; private set; }

        public ImplementationDefinition(Type type, Lifestyle lifestyle)
        {
            Guard.AgainstNull(type, "type");

            if (!Enum.IsDefined(_lifestyleType, lifestyle))
            {
                throw new InvalidOperationException(string.Format(InfrastructureResources.UnknownEnumValueException,
                    _lifestyleType.FullName, lifestyle));
            }

            _constructor = type.GetConstructors().OrderByDescending(item => item.GetParameters().Length).First();
            _constructorParameters = _constructor.GetParameters();

            Type = type;
            Lifestyle = lifestyle;
        }

        public ImplementationDefinition(object instance)
        {
            Guard.AgainstNull(instance, "instance");

            _instance = instance;

            Type = _instance.GetType();
            Lifestyle = Lifestyle.Singleton;
        }

        public object Get(DefaultComponentContainer container)
        {
            lock (_lock)
            {
                switch (Lifestyle)
                {
                    case Lifestyle.Singleton:
                        {
                            return _instance ?? (_instance = CreateInstance(container));
                        }
                    case Lifestyle.Transient:
                        {
                            return CreateInstance(container);
                        }
                    case Lifestyle.Thread:
                        {
                            return _threadInstance ?? (_threadInstance = CreateInstance(container));
                        }
                    default:
                        {
                            throw new InvalidOperationException();
                        }
                }
            }
        }

        private object CreateInstance(DefaultComponentContainer container)
        {
            if (_constructorParameters.Length > 0)
            {
                return Activator.CreateInstance(Type, ResolveConstructorParameters(container).ToArray());
            }
            else
            {
                return Activator.CreateInstance(Type);
            }
        }

        private IEnumerable<object> ResolveConstructorParameters(DefaultComponentContainer container)
        {
            return _constructorParameters.Select(parameter => container.Resolve(parameter.ParameterType));
        }
    }

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

                throw new TypeNotRegisteredException(string.Format(InfrastructureResources.TypeNotRegisteredException, serviceType.FullName));
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
                    throw new DuplicateTypeRegistrationException(
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
                    throw new DuplicateTypeRegistrationException(
                        string.Format(InfrastructureResources.DuplicateTypeRegistrationException,
                            _map[serviceType].Type.FullName, serviceType.FullName, instance.GetType().FullName));
                }
            }

            return this;
        }
    }
}