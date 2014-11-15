using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Shuttle.Core.Infrastructure
{
	public static class TypeExtensions
	{
		public static bool HasInterfaces(this Type type)
		{
			return type.GetInterfaces().Count() > 0;
		}

		public static Type InterfaceMatching(this Type type, string includeRegexPattern)
		{
			return type.InterfaceMatching(includeRegexPattern, string.Empty);
		}

		public static Type InterfaceMatching(this Type type, string includeRegexPattern, string excludeRegexPattern)
		{
			var includeRegex = new Regex(includeRegexPattern, RegexOptions.IgnoreCase);
			Regex excludeRegex = null;
			
			if (!string.IsNullOrEmpty(excludeRegexPattern))
			{
			excludeRegex= new Regex(excludeRegexPattern, RegexOptions.IgnoreCase);
			}

			return (from i in type.GetInterfaces()
			        let fullName = i.FullName
			        where includeRegex.IsMatch(fullName) && (excludeRegex == null || !excludeRegex.IsMatch(fullName))
			        select i).FirstOrDefault();
		}

		public static IEnumerable<Type> FirstInterface(this Type type, Type of)
		{
			var interfaces = type.GetInterfaces();
			var name = string.Format("I{0}", type.Name);

			foreach (var i in interfaces)
			{
				if (i.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
				{
					return new[] { i };
				}
			}

			return (from i in interfaces
					where i.IsAssignableTo(of)
					select new[] { i }).FirstOrDefault();
		}

		/// <summary>
		/// Returns a IEnumerable&lt;Type&gt; containing the interface that has the same name as the type prefixed by an 'I'; else null.
		/// </summary>
		/// <param name="type"></param>
		/// <example>If the type name is Exmaple it will try to locate interface IExample.</example>
		/// <returns></returns>
		public static Type MatchingInterface(this Type type)
		{
			var name = string.Format("I{0}", type.Name);

			return type.GetInterfaces()
				.Where(i => i.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
				.Select(i =>  i ).FirstOrDefault();
		}

		public static IEnumerable<Type> InterfacesAssignableTo<T>(this Type type)
		{
			return type.InterfacesAssignableTo(typeof (T));
		}

		public static IEnumerable<Type> InterfacesAssignableTo(this Type type, Type interfaceType)
		{
			Guard.AgainstNull(interfaceType, "interfaceType");

			return type.GetInterfaces().Where(i => i.IsAssignableTo(interfaceType)).ToList();
		}

		public static void AssertDefaultConstructor(this Type type)
		{
			Guard.AgainstNull(type, "type");

			AssertDefaultConstructor(type, string.Format("Type '{0}' does not have a default constructor.", type.FullName));
		}

		public static void AssertDefaultConstructor(this Type type, string message)
		{
			if (!type.HasDefaultConstructor())
			{
				throw new NotSupportedException(message);
			}
		}

		public static bool HasDefaultConstructor(this Type type)
		{
			return type.GetConstructor(Type.EmptyTypes) != null;
		}

		public static bool IsAssignableTo(this Type type, Type otherType)
		{
			Guard.AgainstNull(type, "type");
			Guard.AgainstNull(otherType, "otherType");

			return type.IsGenericType && otherType.IsGenericType
					? otherType.GetGenericTypeDefinition().IsAssignableFrom(type.GetGenericTypeDefinition())
					: (otherType.IsGenericType
						? IsAssignableToGenericType(type, otherType)
						: otherType.IsAssignableFrom(type));
		}

		private static bool IsAssignableToGenericType(Type type, Type generic)
		{
			return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition().IsAssignableFrom(generic));
		}

		public static Type GetGenericArguments(this Type type, Type generic)
		{
			return (from i in type.GetInterfaces()
					where i.IsGenericType && i.GetGenericTypeDefinition().IsAssignableFrom(generic)
					select i.GetGenericArguments()[0]).FirstOrDefault();
		}
	}
}