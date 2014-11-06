using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace WebServer.Persistence
{
	public static class TypeManager
	{
		private static Dictionary<Assembly, TypeCache> m_typeCaches = new Dictionary<Assembly, TypeCache>();

		private static TypeCache m_nullCache;

		public static TypeCache GetTypeCache(Assembly assembly)
		{
			if (assembly == null)
			{
				if (m_nullCache == null)
					m_nullCache = new TypeCache(null);

				return m_nullCache;
			}

			TypeCache c = null;
			m_typeCaches.TryGetValue(assembly, out c);

			if (c == null)
				m_typeCaches[assembly] = c = new TypeCache(assembly);

			return c;
		}

		public static Type FindTypeByFullName(string fullName)
		{
			return FindTypeByFullName(fullName, true);
		}

		public static Type FindTypeByFullName(string fullName, bool ignoreCase)
		{
			return GetTypeCache(Core.Assembly).GetTypeByFullName(fullName, ignoreCase);
		}

		public static Type FindTypeByName(string name)
		{
			return FindTypeByName(name, true);
		}

		public static Type FindTypeByName(string name, bool ignoreCase)
		{
			return GetTypeCache(Core.Assembly).GetTypeByName(name, ignoreCase);
		}
	}

	public class TypeCache
	{
		public Type[] Types { get; private set; }
		public TypeTable Names { get; private set; }
		public TypeTable FullNames { get; private set; }

		public Type GetTypeByName(string name, bool ignoreCase)
		{
			return Names.Get(name, ignoreCase);
		}

		public Type GetTypeByFullName(string fullName, bool ignoreCase)
		{
			return FullNames.Get(fullName, ignoreCase);
		}

		public TypeCache(Assembly assembly)
		{
			if (assembly == null)
				Types = Type.EmptyTypes;
			else
				Types = assembly.GetTypes();

			Names = new TypeTable(Types.Length);
			FullNames = new TypeTable(Types.Length);

			for (int i = 0; i < Types.Length; ++i)
			{
				Type type = Types[i];
				Names.Add(type.Name, type);
				FullNames.Add(type.FullName, type);
			}
		}
	}

	public class TypeTable
	{
		private Dictionary<string, Type> m_sensitive;
		private Dictionary<string, Type> m_insensitive;

		public void Add(string key, Type type)
		{
			m_sensitive[key] = type;
			m_insensitive[key] = type;
		}

		public Type Get(string key, bool ignoreCase)
		{
			Type t = null;

			if (ignoreCase)
				m_insensitive.TryGetValue(key, out t);
			else
				m_sensitive.TryGetValue(key, out t);

			return t;
		}

		public TypeTable(int capacity)
		{
			m_sensitive = new Dictionary<string, Type>(capacity);
			m_insensitive = new Dictionary<string, Type>(capacity, StringComparer.OrdinalIgnoreCase);
		}
	}
}
