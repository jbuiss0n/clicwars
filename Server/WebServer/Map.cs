using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using TileEngine;
using WebServer.Network;
using WebServer.Network.Packet;

namespace WebServer
{
	public class Map
	{
		private List<Mobile> m_mobiles;

		private Point m_startLocation;

		public int Id { get; private set; }

		public string Name { get; private set; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		public int TotalWidth { get { return Width * Set.TileWidth; } }

		public List<Tile[,]> Layers { get; private set; }

		public TileSet Set { get; private set; }

		public Map(int id, string name, TileSet set)
		{
			m_mobiles = new List<Mobile>();

			Layers = new List<Tile[,]>();

			Id = id;
			Name = name;
			Set = set;

			SetStartLocation(new Point(10 * Set.TileWidth, 10 * Set.TileHeight));//FIXME: should be in MapDatas
		}

		public void LoadTiles(List<List<List<int>>> layers)
		{
			Width = layers[0][0].Count;
			Height = layers[0].Count;

			for (int i = 0; i < layers.Count; i++)
			{
				var layer = new Tile[Width, Height];

				for (int x = 0; x < Width; x++)
				{
					for (int y = 0; y < Height; y++)
					{
						layer[x, y] = Set[layers[i][y][x]];
					}
				}

				Layers.Add(layer);
			}
		}

		public void SetStartLocation(Point location)
		{
			m_startLocation = location;
		}

		public Point GetStartLocation()
		{
			var location = m_startLocation;
			var i = 0;

			while (!CanMove(null, location))
			{
				var j = (int)Math.Ceiling(i / 8.0);
				var k = i % 8.0;

				if (k == 0 || k == 1 || k == 7)
				{
					location.X += 1 * j;
				}
				else if (k == 3 || k == 4 || k == 5)
				{
					location.X -= 1 * j;
				}

				if (k == 1 || k == 2 || k == 3)
				{
					location.Y += 1 * j;
				}
				else if (k == 5 || k == 6 || k == 7)
				{
					location.Y -= 1 * j;
				}

				i++;
			}

			return location;
		}

		public bool CanMove(Mobile mobile, Point newLocation)
		{
			for (int i = 0; i < Layers.Count; i++)
			{
				if (GetTileFromPosition(i, newLocation.X, newLocation.Y).Flag.HasFlag(TileFlag.Impassable))
					return false;
			}

			var objectInRange = GetObjectsInRange(newLocation, Set.TileWidth > Set.TileHeight ? Set.TileWidth : Set.TileHeight);
			foreach (var entity in objectInRange)
			{
				if (entity != mobile
					&& newLocation.X > entity.Location.X - entity.Width
					&& newLocation.X < entity.Location.X + entity.Width
					&& newLocation.Y > entity.Location.Y - entity.Height
					&& newLocation.Y < entity.Location.Y + entity.Height)
				{
					return false;
				}
			}

			return true;
		}

		public void OnMove(Point oldLocation, Mobile mobile)
		{

		}

		public void OnEnter(Mobile mobile)
		{
			m_mobiles.Add(mobile);
		}

		public void OnLeave(Mobile mobile)
		{
			m_mobiles.Remove(mobile);
		}

		public virtual void OnDidHarmful(Mobile harmer, Mobile harmed)
		{
		}

		public virtual void OnGotHarmful(Mobile harmer, Mobile harmed)
		{
		}

		public virtual bool OnDamage(Mobile mobile, ref int amount)
		{
			return true;
		}

		public virtual bool OnBeforeDeath(Mobile victim, Mobile killer)
		{
			return true;
		}

		public virtual void OnDeath(Mobile victim, Mobile killer)
		{
		}

		public virtual List<IEntity> GetObjectsInRange(Point location, int range)
		{
			var entitiesInRange = new List<IEntity>();

			entitiesInRange.AddRange(GetMobilesInRange(location, range));

			return entitiesInRange;
		}

		public virtual List<Mobile> GetMobilesInRange(Point location, int range)
		{
			var entitiesInRange = new List<Mobile>();

			foreach (var mobile in m_mobiles)
			{
				if (Utility.InRange(location, mobile.Location, range))
				{
					entitiesInRange.Add(mobile);
				}
			}
			return entitiesInRange;
		}

		public bool CanShootThrough(Point point)
		{
			for (int i = 0; i < Layers.Count; i++)
			{
				if (GetTileFromPosition(i, point.X, point.Y).Flag.HasFlag(TileFlag.NoShoot))
					return false;
			}

			return true;
		}

		private IEnumerable<Tile> GetTilesFromPosition(double x, double y)
		{
			for (int l = 0; l < Layers.Count; l++)
			{
				yield return GetTileFromPosition(l, x, y);
			}
		}

		private Tile GetTileFromPosition(int l, double x, double y)
		{
			return Layers[l][(int)Math.Floor(x / Set.TileWidth), (int)Math.Floor(y / Set.TileHeight)];
		}

		public bool LineOfSight(Mobile from, Mobile to)
		{
			return LineOfSight(from.Location, to.Location);
		}

		public bool LineOfSight(Point from, Point to)
		{
			if (!Utility.InUpdateRange(from, to))
				return false;

			if (from == to)
				return true;

			Point p;

			var path = new List<Point>();
			var start = from;
			var end = to;

			if (from.X > to.X || (from.X == to.X && from.Y > to.Y))
			{
				Point swap = from;
				from = to;
				to = swap;
			}

			var deltaX = to.X - from.X;
			var deltaY = to.Y - from.Y;

			var x = from.X;
			var y = from.Y;
			var d = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

			var xd = deltaX / d;
			var xy = deltaY / d;

			while (Utility.NumberBetween(x, to.X, from.X, 0.5) && Utility.NumberBetween(y, to.Y, from.Y, 0.5))
			{
				var ix = (int)Math.Round(x);
				var iy = (int)Math.Round(y);

				if (path.Count > 0)
				{
					p = path.Last();

					if (p.X != ix || p.Y != iy)
						path.Add(new Point(ix, iy));
				}
				else
				{
					path.Add(new Point(ix, iy));
				}
				x += xd;
				y += xy;
			}

			if (path.Count == 0)
				return true;//<--should never happen, but to be safe.

			p = path.Last();

			if (p != to)
				path.Add(to);

			Point pTop = from;
			Point pBottom = to;

			Utility.FixPoints(ref pTop, ref pBottom);

			foreach (var point in path)
			{
				var tiles = GetTilesFromPosition(point.X, point.Y);

				foreach (var tile in tiles)
				{
					if (tile.Flag.HasFlag(TileFlag.BlockingSight))
						return false;
				}
			}

			//TODO : Check tout les items / mobiles sur le chemin et voir s'il bloque la vue ou non

			return true;
		}

		public Point GetPointInRange(double x, double y)
		{
			return new Point(Math.Max(0, (int)Math.Round(x)), Math.Min((int)Math.Round(y), TotalWidth));
		}

		public void Broadcast(Packet packet)
		{
			foreach (var mobile in m_mobiles)
			{
				var player = mobile as PlayerMobile;
				if (player != null)
					player.SendPacket(packet);
			}
		}

	}
}
