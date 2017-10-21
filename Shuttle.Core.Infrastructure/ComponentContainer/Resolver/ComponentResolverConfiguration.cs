using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Shuttle.Core.Infrastructure
{
    public class ComponentResolverConfiguration : IComponentResolverConfiguration
    {
        private readonly List<Component> _components = new List<Component>();

        public IEnumerable<Component> Components => new ReadOnlyCollection<Component>(_components);

        public void AddComponent(Component component)
        {
            Guard.AgainstNull(component, nameof(component));

            if (Contains(component))
            {
                return;
            }

            _components.Add(component);
        }

        public bool Contains(Component component)
        {
            Guard.AgainstNull(component, nameof(component));

            return _components.Find(item => item.DependencyType == component.DependencyType) != null;
        }

        public class Component
        {
            public Component(Type dependencyType)
            {
                Guard.AgainstNull(dependencyType, nameof(dependencyType));

                DependencyType = dependencyType;
            }

            public Type DependencyType { get; }
        }
    }
}