using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServer.Network;
using WebServer.Network.Packet;
using TileEngine;
using WebServer.Persistence;
using WebServer.Accounting;

namespace WebServer
{
	public static class World
	{
		static World()
		{
			s_mobiles = new Dictionary<Serial, Mobile>();
			s_maps = new List<Map>();
		}

		public static int GLOBAL_MAX_UPDATE_RANGE = 400;

		public static TimeSpan GLOBAL_UPDATE_TIME = TimeSpan.FromMilliseconds(15);

		private static Dictionary<Serial, Mobile> s_mobiles;

		private static List<Map> s_maps;

		public static void Load()
		{
			s_maps = WorldPersistence.LoadMaps();
			s_mobiles = WorldPersistence.LoadMobiles();

			EventSink.InvokeWorldLoad();
		}

		public static void Save()
		{
			WorldPersistence.Backup();

			WorldPersistence.SaveMobiles(s_mobiles);

			EventSink.InvokeWorldSave();
		}

		public static void Broadcast(Packet packet)
		{
			var list = Client.Clients;

			foreach (var client in list)
			{
				client.Send(packet);
			}
		}

		public static Mobile FindMobile(Serial serial)
		{
			Mobile mobile;

			s_mobiles.TryGetValue(serial, out mobile);

			return mobile;
		}

		public static Map FindMap(int id)
		{
			return s_maps[id];
		}

		public static void AddMobile(Mobile mobile)
		{
			s_mobiles[mobile.Serial] = mobile;
		}

		public static void RemoveMobile(Mobile mobile)
		{
			s_mobiles.Remove(mobile.Serial);
		}

		public static bool OnDelete(IEntity entity)
		{
			return true;
		}
	}
}
