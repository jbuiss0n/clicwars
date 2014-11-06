using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;

namespace WebServer.Persistence
{
	public class BinaryWriter : System.IO.BinaryWriter
	{
		public BinaryWriter(System.IO.Stream output)
			: base(output)
		{
		}

		public void Write(DateTime dateTime)
		{
			Write(dateTime.Ticks);
		}

		public void Write(Point point)
		{
			Write(point.X);
			Write(point.Y);
		}

		public void Write(Map map)
		{
			Write(map.Id);
		}

		public void Write(Mobile mobile)
		{
			Write((Int32)mobile.Serial);
		}
	}
}