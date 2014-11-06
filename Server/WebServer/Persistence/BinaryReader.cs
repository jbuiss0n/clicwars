using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;

namespace WebServer.Persistence
{
	public class BinaryReader : System.IO.BinaryReader
	{
		public BinaryReader(System.IO.Stream input)
			: base(input)
		{
		}

		public DateTime ReadDateTime()
		{
			var ticks = ReadInt64();
			return new DateTime(ticks);
		}

		public Point ReadPoint()
		{
			var x = ReadDouble();
			var y = ReadDouble();
			return new Point(x, y);
		}

		public Map ReadMap()
		{
			var id = ReadInt32();
			return World.FindMap(id);
		}

		public Mobile ReadMobile()
		{
			var serial = ReadInt32();
			return World.FindMobile(serial);
		}
	}
}
