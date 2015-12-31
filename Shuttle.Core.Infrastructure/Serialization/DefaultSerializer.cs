using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Shuttle.Core.Infrastructure.Serialization
{
	public class DefaultSerializer : ISerializer, ISerializerTypes
	{
		private static readonly object Padlock = new object();
		private readonly XmlSerializerNamespaces _namespaces = new XmlSerializerNamespaces();

		private readonly List<Type> _serializerTypes = new List<Type>();
		private readonly XmlWriterSettings _xmlSettings;
		private readonly XmlAttributeOverrides _overrides = new XmlAttributeOverrides();

		private readonly Dictionary<string, XmlSerializer> _serializers = new Dictionary<string, XmlSerializer>();

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

			AddSerializerType(messageType);

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

		public void AddSerializerType(Type type)
		{
			Guard.AgainstNull(type, "type");

			if (HasSerializerType(type))
			{
				return;
			}

			lock (Padlock)
			{
				if (HasSerializerType(type))
				{
					return;
				}

				_serializerTypes.Add(type);
				_overrides.Add(type, new XmlAttributes { XmlRoot = new XmlRootAttribute { Namespace = type.Namespace } });

				foreach (var nested in type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (!HasSerializerType(nested))
					{
						AddSerializerType(nested);
					}
				}

				_serializers.Clear();
			}
		}

		public bool HasSerializerType(Type type)
		{
			lock (Padlock)
			{
				return
					_serializerTypes.Find(
						candidate =>
							(candidate.AssemblyQualifiedName ?? string.Empty).Equals(type.AssemblyQualifiedName,
								StringComparison.InvariantCultureIgnoreCase)) != null;
			}
		}

		private XmlSerializer GetSerializer(Type type)
		{
			lock (Padlock)
			{
				var key = type.AssemblyQualifiedName;

				if (string.IsNullOrEmpty(key))
				{
					throw new ApplicationException();
				}

				if (!_serializers.ContainsKey(key))
				{
					_serializers.Add(key, new XmlSerializer(type, _overrides, _serializerTypes.ToArray(), null, string.Empty));
				}

				return _serializers[key];
			}
		}
	}
}