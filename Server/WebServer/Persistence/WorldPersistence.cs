using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using TileEngine;
using System.IO;
using System.Xml;

namespace WebServer.Persistence
{
	public static class WorldPersistence
	{
		private const string MAP_LAYER_FILE_EXTENSION = ".layer";
		private const string MAP_SPAWNING_FILE_NAME = "spawning.xml";

		public static void Backup()
		{
			var savesPath = PathManager.GetFullPath(PathManager.PATH_SAVES);
			var backupsPath = PathManager.GetFullPath(Path.Combine(PathManager.PATH_BACKUPS, DateTime.Now.Ticks.ToString()));

			Directory.Move(savesPath, backupsPath);
		}

		#region MAPS
		public static List<Map> LoadMaps()
		{
			var idxPath = PathManager.GetFullPath(PathManager.PATH_MAP_INDEX);

			PathManager.EnsureDirectory(idxPath);

			var idxReader = new BinaryReader(new FileStream(idxPath, FileMode.Open));

			var entries = new List<MapEntry>();

			var mapCount = idxReader.ReadInt32();
			for (int i = 0; i < mapCount; i++)
			{
				var tileset = idxReader.ReadString();
				var name = idxReader.ReadString();

				entries.Add(new MapEntry(i, name, tileset));
			}

			var maps = new List<Map>();
			foreach (var entry in entries)
			{
				maps.Add(LoadMap(entry));
			}

			idxReader.Close();

			return maps;
		}

		private static Map LoadMap(MapEntry entry)
		{
			var mapPath = PathManager.GetFullPath(Path.Combine(PathManager.PATH_MAP_DATA, String.Format("{0}-{1}", entry.Index, entry.Name)));

			PathManager.EnsureDirectory(mapPath);

			var layers = new List<List<List<int>>>();
			var layerIndex = 0;
			var layerPath = Path.Combine(mapPath, (layerIndex++) + MAP_LAYER_FILE_EXTENSION);

			while (File.Exists(layerPath))
			{
				layers.Add(LoadLayer(layerPath));
				layerPath = Path.Combine(mapPath, (layerIndex++) + MAP_LAYER_FILE_EXTENSION);
			}

			var map = new Map(entry.Index, entry.Name, TileSet.Get(entry.Tileset));
			map.LoadTiles(layers);

			return map;
		}

		private static List<List<int>> LoadLayer(string layerPath)
		{
			var layer = new List<List<int>>();
			var rows = File.ReadAllLines(layerPath);

			foreach (var row in rows)
			{
				layer.Add(new List<int>());
				foreach (var column in row.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
				{
					layer.Last().Add(column != "-" ? Int32.Parse(column) : -1);
				}
			}

			return layer;
		}

		public static void SaveMaps(List<Map> maps)
		{
			var idxPath = PathManager.GetFullPath(PathManager.PATH_MAP_INDEX);
			var idxWritter = new BinaryWriter(new FileStream(idxPath, FileMode.Create));

			idxWritter.Write(maps.Count);

			foreach (var map in maps)
			{
				idxWritter.Write(map.Set.Name);
				idxWritter.Write(map.Name);
			}

			idxWritter.Flush();
			idxWritter.Close();
		}
		#endregion

		public static Dictionary<Serial, Mobile> LoadMobiles()
		{
			var idxPath = PathManager.GetFullPath(Path.Combine(PathManager.PATH_SAVES, PathManager.PATH_MOBILE_INDEX));
			var tdbPath = PathManager.GetFullPath(Path.Combine(PathManager.PATH_SAVES, PathManager.PATH_MOBILE_TYPES));
			var binPath = PathManager.GetFullPath(Path.Combine(PathManager.PATH_SAVES, PathManager.PATH_MOBILE_DATA));

			return Load<Mobile>(idxPath, tdbPath, binPath);
		}

		public static void SaveMobiles(Dictionary<Serial, Mobile> mobiles)
		{
			var idxPath = PathManager.GetFullPath(Path.Combine(PathManager.PATH_SAVES, PathManager.PATH_MOBILE_INDEX));
			var tbdPath = PathManager.GetFullPath(Path.Combine(PathManager.PATH_SAVES, PathManager.PATH_MOBILE_TYPES));
			var binPath = PathManager.GetFullPath(Path.Combine(PathManager.PATH_SAVES, PathManager.PATH_MOBILE_DATA));

			Save(mobiles, idxPath, tbdPath, binPath);
		}

		private static Dictionary<Serial, T> Load<T>(string idxPath, string tdbPath, string binPath)
			where T : IEntity, new()
		{
			PathManager.EnsureDirectory(idxPath);
			PathManager.EnsureDirectory(tdbPath);
			PathManager.EnsureDirectory(binPath);

			var elements = new Dictionary<Serial, T>();

			var idxReader = new BinaryReader(new FileStream(idxPath, FileMode.Open));
			var tdbReader = new BinaryReader(new FileStream(tdbPath, FileMode.Open));
			var binReader = new BinaryReader(new FileStream(binPath, FileMode.Open));

			var types = new List<Type>();
			var entries = new List<EntityEntry<T>>();

			var typesCount = tdbReader.ReadInt32();
			for (int i = 0; i < typesCount; i++)
			{
				var typeName = tdbReader.ReadString();
				types.Add(TypeManager.FindTypeByFullName(typeName));
			}

			var entriesCount = idxReader.ReadInt32();
			for (int i = 0; i < entriesCount; i++)
			{
				var typeIndex = idxReader.ReadInt32();
				var serial = idxReader.ReadInt32();
				var start = idxReader.ReadInt64();
				var length = idxReader.ReadInt32();

				var mobile = (T)Activator.CreateInstance(types[typeIndex], new object[] { (Serial)serial });

				entries.Add(new EntityEntry<T>(mobile, typeIndex, start, length));
			}

			foreach (var entry in entries)
			{
				binReader.BaseStream.Seek(entry.Position, SeekOrigin.Begin);
				entry.Entity.Deserialize(binReader);

				if (binReader.BaseStream.Position != (entry.Position + entry.Length))
				{
					throw new Exception(String.Format("***** Bad deserialize on {0} *****", entry.Entity.GetType()));
				}

				elements.Add(entry.Serial, entry.Entity);
			}

			idxReader.Close();
			tdbReader.Close();
			binReader.Close();

			return elements;
		}

		private static void Save<T>(Dictionary<Serial, T> toSave, string idxPath, string tbdPath, string binPath)
			where T : IEntity, new()
		{
			var elements = toSave.Values;
			var types = new List<Type>();

			PathManager.EnsureDirectory(idxPath);
			PathManager.EnsureDirectory(tbdPath);
			PathManager.EnsureDirectory(binPath);

			var idxWritter = new BinaryWriter(new FileStream(idxPath, FileMode.Create));
			var tbdWritter = new BinaryWriter(new FileStream(tbdPath, FileMode.Create));
			var binWritter = new BinaryWriter(new FileStream(binPath, FileMode.Create));

			idxWritter.Write(elements.Count());

			foreach (T mobile in elements)
			{
				var typeIndex = GetTypeIndex(types, mobile);
				var start = binWritter.BaseStream.Position;

				idxWritter.Write((int)typeIndex);
				idxWritter.Write((int)mobile.Serial);
				idxWritter.Write((long)start);

				mobile.Serialize(binWritter);

				idxWritter.Write((int)(binWritter.BaseStream.Position - start));
			}

			tbdWritter.Write(types.Count);
			for (int i = 0; i < types.Count; ++i)
			{
				tbdWritter.Write(types[i].FullName);
			}

			idxWritter.Flush();
			tbdWritter.Flush();
			binWritter.Flush();

			idxWritter.Close();
			tbdWritter.Close();
			binWritter.Close();
		}

		private static int GetTypeIndex<T>(List<Type> types, T mobile)
			where T : IEntity, new()
		{
			var type = mobile.GetType();
			var typeIndex = types.IndexOf(type);

			if (typeIndex == -1)
			{
				types.Add(type);
				typeIndex = types.Count - 1;
			}
			return typeIndex;
		}
	}
}