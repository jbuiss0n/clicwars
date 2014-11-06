using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace WebServer.Persistence
{
	public static class PathManager
	{
		public const string PATH_BACKUPS = "Backups";
		public const string PATH_SAVES = "Saves";

		public const string PATH_ACCOUNTS = "Accounts/Accounts.xml";

		public const string PATH_MOBILE_INDEX = "Mobiles/Mobiles.idx";
		public const string PATH_MOBILE_TYPES = "Mobiles/Mobiles.tdb";
		public const string PATH_MOBILE_DATA = "Mobiles/Mobiles.bin";

		public const string PATH_ITEM_INDEX = "Items/Items.idx";
		public const string PATH_ITEM_TYPES = "Items/Items.tdb";
		public const string PATH_ITEM_DATA = "Items/Items.bin";

		public const string PATH_MAP_INDEX = "Maps/Maps.idx";
		public const string PATH_MAP_DATA = "Maps";

		private static string BASE_DIRECTORY = ConfigurationManager.AppSettings["Path_GameData"];

		public static string BaseDirectory { get { return BASE_DIRECTORY; } }

		public static string GetFullPath(string path)
		{
			return Path.Combine(BASE_DIRECTORY, path);
		}

		public static void EnsureDirectory(string path)
		{
			var directory = path;

			if (Path.HasExtension(directory))
				directory = Path.GetDirectoryName(directory);

			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);
		}
	}
}
