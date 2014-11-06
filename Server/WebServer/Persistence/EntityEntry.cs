using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServer.Persistence
{
	public sealed class EntityEntry<T>
		where T : IEntity
	{
		public T Entity { get; private set; }

		public int TypeIndex { get; private set; }

		public long Position { get; private set; }

		public int Length { get; private set; }

		public Serial Serial
		{
			get
			{
				return Entity == null ? Serial.MinusOne : Entity.Serial;
			}
		}

		public EntityEntry(T entity, int typeIndex, long position, int length)
		{
			Entity = entity;
			TypeIndex = typeIndex;
			Position = position;
			Length = length;
		}
	}

	public sealed class MapEntry
	{
		public int Index { get; private set; }

		public string Name { get; private set; }

		public string Tileset { get; private set; }

		public MapEntry(int index, string name, string tileset)
		{
			Index = index;
			Name = name;
			Tileset = tileset;
		}
	}
}
