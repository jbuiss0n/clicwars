using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using TileEngine;
using WebServer.Persistence;

namespace WebServer
{
	public interface IEntity : IComparable, IComparable<IEntity>
	{
		Serial Serial { get; }
		Point Location { get; }
		int Width { get; }
		int Height { get; }
		Map Map { get; }

		bool Deleted { get; }

		void Serialize(BinaryWriter binWritter);
		void Deserialize(BinaryReader binReader);
	}
}