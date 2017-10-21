using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Shuttle.Core.Infrastructure
{
    public class DefaultSerializer : ISerializer, ISerializerRootType
    {
        private static readonly object Padlock = new object();
        private readonly XmlSerializerNamespaces _namespaces = new XmlSerializerNamespaces();

        private readonly Dictionary<Type, XmlAttributeOverrides> _overrides =
            new Dictionary<Type, XmlAttributeOverrides>();

        private readonly Dictionary<Type, XmlSerializer> _serializers = new Dictionary<Type, XmlSerializer>();

        private readonly XmlWriterSettings _xmlSettings;

        public DefaultSerializer()
        {
            _xmlSettings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true,
                Indent = true
            };

            _namespaces.Add(string.Empty, string.Empty);
        }

        public Stream Serialize(object instance)
        {
            Guard.AgainstNull(instance, "instance");

            var messageType = instance.GetType();
            var serializer = GetSerializer(messageType);

            var xml = new StringBuilder();

            using (var writer = XmlWriter.Create(xml, _xmlSettings))
            {
                serializer.Serialize(writer, instance, _namespaces);

                writer.Flush();
            }

            return new MemoryStream(Encoding.UTF8.GetBytes(xml.ToString()));
        }

        public object Deserialize(Type type, Stream stream)
        {
            Guard.AgainstNull(type, "type");
            Guard.AgainstNull(stream, "stream");

            using (var copy = stream.Copy())
            using (var reader = XmlDictionaryReader.CreateTextReader(copy, Encoding.UTF8,
                new XmlDictionaryReaderQuotas
                {
                    MaxArrayLength = int.MaxValue,
                    MaxStringContentLength = int.MaxValue,
                    MaxNameTableCharCount = int.MaxValue
                }, null))
            {
                return GetSerializer(type).Deserialize(reader);
            }
        }

        public void AddSerializerType(Type root, Type contained)
        {
            Guard.AgainstNull(root, "type");
            Guard.AgainstNull(contained, "contained");

            if (HasSerializerType(root, contained))
            {
                return;
            }

            lock (Padlock)
            {
                if (HasSerializerType(root, contained))
                {
                    return;
                }

                if (!_overrides.ContainsKey(root))
                {
                    _overrides.Add(root, new XmlAttributeOverrides());
                }

                var overrides = _overrides[root];

                overrides.Add(contained,
                    new XmlAttributes {XmlRoot = new XmlRootAttribute {Namespace = contained.Namespace}});

                foreach (var nested in root.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (!HasSerializerType(root, nested))
                    {
                        AddSerializerType(root, nested);
                    }
                }

                _serializers.Clear();
            }
        }

        private bool HasSerializerType(Type root, Type contained)
        {
            lock (Padlock)
            {
                return _overrides.ContainsKey(root) && _overrides[root][contained] != null;
            }
        }

        private XmlSerializer GetSerializer(Type type)
        {
            lock (Padlock)
            {
                if (!_overrides.ContainsKey(type))
                {
                    _overrides.Add(type, new XmlAttributeOverrides());
                }

                if (!_serializers.ContainsKey(type))
                {
                    _serializers.Add(type, new XmlSerializer(type, _overrides[type]));
                }

                return _serializers[type];
            }
        }
    }
}