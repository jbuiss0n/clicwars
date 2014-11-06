using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
	public class Tile
	{
		public static Tile Empty = new Tile(TileFlag.None, TileTexture.None);

		public TileFlag Flag { get; private set; }

		public TileTexture Texture { get; private set; }

		public Tile(TileFlag flag, TileTexture texture)
		{
			Flag = flag;
			Texture = texture;
		}
	}
	
	[Flags]
	public enum TileFlag
	{
		None			= 0x0000,
		BlockingSight	= 0x0001,
		Impassable		= 0x0002,
		NoShoot			= 0x0004,
	}

	[Flags]
	public enum TileTexture
	{
		None	= 0x00,
		Earth	= 0x01,
		Grass	= 0x02,
		Stone	= 0x04,
		Water	= 0x08,
		Bush	= 0x16,
		Floor	= 0x32,
		Wall	= 0x64,
	}
}
