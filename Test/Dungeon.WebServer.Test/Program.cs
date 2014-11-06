using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

using TileEngine;
using WebServer;
using WebServer.Accounting;
using WebServer.Network;
using WebServer.Network.Packet;
using WebServer.Services;

namespace Dungeon.WebServer.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Starting");

			using (ServiceHost host = new ServiceHost(typeof(ServerAdminService)))
			{
				host.Open();
				Console.WriteLine("ServiceHost open");

				var task = Task.Factory.StartNew(() => { Core.Setup(); Core.Start(); });
				Console.WriteLine("Core starting");

				var input = Console.ReadLine();
				while (input != "exit")
				{
					if (input == "save")
						World.Save();
					else if (input == "load")
						World.Load();

					input = Console.ReadLine();
				}

				Core.Stop();

				World.Save();

				host.Close();
			}
		}
	}
}
