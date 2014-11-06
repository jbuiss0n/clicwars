using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
	public class TileSet
	{
		private static List<TileSet> s_tilesets;

		static TileSet()
		{
			s_tilesets = new List<TileSet>
			{
				new TileSet
				{
					Id = 1,
					Name = "Outside",
					TileHeight = 48,
					TileWidth = 48,
					m_tiles = new List<Tile>
					{
						new Tile(TileFlag.None, TileTexture.Grass),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.Impassable, TileTexture.Water),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.None),
						new Tile(TileFlag.None, TileTexture.None),
						new Tile(TileFlag.None, TileTexture.None),
						new Tile(TileFlag.None, TileTexture.None),
						new Tile(TileFlag.None, TileTexture.None),
						
						new Tile(TileFlag.None, TileTexture.Grass),
						new Tile(TileFlag.None, TileTexture.Grass),
						new Tile(TileFlag.None, TileTexture.Grass),
						new Tile(TileFlag.None, TileTexture.Grass),
						new Tile(TileFlag.None, TileTexture.Grass),
						new Tile(TileFlag.None, TileTexture.Grass),
						new Tile(TileFlag.None, TileTexture.Grass),
						new Tile(TileFlag.None, TileTexture.Grass),
						new Tile(TileFlag.None, TileTexture.Grass),
						new Tile(TileFlag.None, TileTexture.Grass),
						new Tile(TileFlag.None, TileTexture.Grass),
						new Tile(TileFlag.None, TileTexture.Grass),
						
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.None, TileTexture.Earth),
						
						new Tile(TileFlag.Impassable, TileTexture.Water),
						new Tile(TileFlag.Impassable, TileTexture.Water),
						new Tile(TileFlag.Impassable, TileTexture.Water),
						new Tile(TileFlag.Impassable, TileTexture.Water),
						new Tile(TileFlag.Impassable, TileTexture.Water),
						new Tile(TileFlag.Impassable, TileTexture.Water),
						new Tile(TileFlag.Impassable, TileTexture.Water),
						new Tile(TileFlag.Impassable, TileTexture.Water),
						new Tile(TileFlag.Impassable, TileTexture.Water),
						new Tile(TileFlag.Impassable, TileTexture.Water),
						new Tile(TileFlag.Impassable, TileTexture.Water),
						new Tile(TileFlag.Impassable, TileTexture.Water),
						
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						new Tile(TileFlag.None, TileTexture.Floor),
						
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Wall),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Wall),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Wall),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Wall),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Wall),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Wall),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Wall),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Wall),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Wall),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Wall),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Wall),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Wall),
						
						new Tile(TileFlag.Impassable, TileTexture.Bush),
						new Tile(TileFlag.Impassable, TileTexture.Bush),
						new Tile(TileFlag.Impassable, TileTexture.Bush),
						new Tile(TileFlag.None, TileTexture.Earth),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Stone),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Stone),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Stone),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Stone),
						new Tile(TileFlag.Impassable | TileFlag.NoShoot, TileTexture.Stone),
						new Tile(TileFlag.None, TileTexture.Bush),
						new Tile(TileFlag.None, TileTexture.None),
						new Tile(TileFlag.None, TileTexture.None),
					}
				}
			};
		}

		public static TileSet Get(string name)
		{
			return s_tilesets.FirstOrDefault(t => t.Name == name);
		}

		private List<Tile> m_tiles;

		public int Id { get; set; }
		public string Name { get; set; }

		public int TileWidth { get; private set; }
		public int TileHeight { get; private set; }

		private TileSet()
		{
			m_tiles = new List<Tile>();
		}

		public Tile this[int id]
		{
			get
			{
				if (id < 0 || id > m_tiles.Count)
					return Tile.Empty;

				return m_tiles[id];
			}
		}
	}
}
