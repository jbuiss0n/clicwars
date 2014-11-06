using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServer.Services.Accounting
{
	public class CharacterResult
	{
		public int Serial { get; set; }

		public string Name { get; set; }

		public int Body { get; set; }

		public int Deaths { get; set; }

		public int Kills { get; set; }

		public DateTime CreationDate { get; set; }
	}
}
