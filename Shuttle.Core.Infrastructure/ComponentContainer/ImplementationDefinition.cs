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
        private static Dictionary<Type, object> _threadInstances;

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
                        if (!GuardedThreadInstances().ContainsKey(Type))
                        {
                            GuardedThreadInstances().Add(Type, CreateInstance(container));
                        }

                        return GuardedThreadInstances()[Type];
                    }
                    default:
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }

        private Dictionary<Type, object> GuardedThreadInstances()
        {
            return _threadInstances ?? (_threadInstances = new Dictionary<Type, object>());
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
}