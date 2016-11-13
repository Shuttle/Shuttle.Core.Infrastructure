using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Shuttle.Core.Infrastructure
{
    public class ImplementationDefinition : IDisposable
    {
        private readonly Type _lifestyleType = typeof (Lifestyle);
        private readonly object _lock = new object();
        private object _instance;

        private readonly Dictionary<Type, Dictionary<int, object>> _threadInstances =
            new Dictionary<Type, Dictionary<int, object>>();

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

            _constructorParameters =
                type.GetConstructors().OrderByDescending(item => item.GetParameters().Length).First().GetParameters();

            Type = type;
            Lifestyle = lifestyle;

            if (lifestyle == Lifestyle.Thread && !_threadInstances.ContainsKey(type))
            {
                _threadInstances.Add(Type, new Dictionary<int, object>());
            }
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
                        var instances = _threadInstances[Type];
                        var managedThreadId = Thread.CurrentThread.ManagedThreadId;

                        if (!instances.ContainsKey(managedThreadId))
                        {
                            instances.Add(managedThreadId, CreateInstance(container));
                        }

                        return instances[managedThreadId];
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

        public void Dispose()
        {
            switch (Lifestyle)
            {
                case Lifestyle.Singleton:
                {
                    _instance.AttemptDispose();

                    break;
                }
                case Lifestyle.Thread:
                {
                    foreach (var o in _threadInstances[Type])
                    {
                        o.AttemptDispose();
                    }

                    break;
                }
            }
        }
    }
}