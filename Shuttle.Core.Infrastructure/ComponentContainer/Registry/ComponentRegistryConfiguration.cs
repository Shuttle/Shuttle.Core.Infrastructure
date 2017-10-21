using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Shuttle.Core.Infrastructure
{
    public class ComponentRegistryConfiguration : IComponentRegistryConfiguration
    {
        private readonly List<Component> _components = new List<Component>();
        private readonly List<Collection> _collections = new List<Collection>();

        public IEnumerable<Component> Components => new ReadOnlyCollection<Component>(_components);
        public IEnumerable<Collection> Collections => new ReadOnlyCollection<Collection>(_collections);

        public void AddComponent(Component component)
        {
            Guard.AgainstNull(component, nameof(component));

            _components.Add(component);
        }

        public class Component
        {
            public Component(Type dependencyType, Type implementationType, Lifestyle lifestyle)
            {
                Guard.AgainstNull(dependencyType, nameof(dependencyType));
                Guard.AgainstNull(implementationType, nameof(implementationType));

                DependencyType = dependencyType;
                ImplementationType = implementationType;
                Lifestyle = lifestyle;
            }

            public Type DependencyType { get; }
            public Type ImplementationType { get; }
            public Lifestyle Lifestyle { get; }
        }

        public class Collection
        {
            private readonly List<Type> _implementationTypes;

            public Collection(Type dependencyType, IEnumerable<Type> implementationTypes, Lifestyle lifestyle)
            {
                Guard.AgainstNull(dependencyType, nameof(dependencyType));
                Guard.AgainstNull(implementationTypes, nameof(implementationTypes));

                DependencyType = dependencyType;
                Lifestyle = lifestyle;

                _implementationTypes = new List<Type>(implementationTypes);

                if (!_implementationTypes.Any())
                {
                    throw new InvalidOperationException(
                        string.Format(InfrastructureResources.EmptyCollectionImplementationTypes,
                            dependencyType.FullName));
                }
            }

            public Type DependencyType { get; }
            public Lifestyle Lifestyle { get; }

            public IEnumerable<Type> ImplementationTypes => new ReadOnlyCollection<Type>(_implementationTypes);
        }

        public void AddCollection(Collection collection)
        {
            Guard.AgainstNull(collection, nameof(collection));

            _collections.Add(collection);
        }
    }
}