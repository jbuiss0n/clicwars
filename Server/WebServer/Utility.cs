using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;

namespace WebServer
{
	public static class Utility
	{
		public static double COS45 = Math.Cos(Math.PI * 45 / 180);

		#region Random
		private static Random m_random = new Random();

		public static double RandomDouble()
		{
			return m_random.NextDouble();
		}

		public static bool RandomBool()
		{
			return (m_random.Next(2) == 0);
		}

		public static int RandomMinMax(int min, int max)
		{
			if (min > max)
			{
				int copy = min;
				min = max;
				max = copy;
			}
			else if (min == max)
			{
				return min;
			}

			return min + m_random.Next((max - min) + 1);
		}

		public static int Random(int from, int count)
		{
			if (count == 0)
			{
				return from;
			}
			else if (count > 0)
			{
				return from + m_random.Next(count);
			}
			else
			{
				return from - m_random.Next(-count);
			}
		}

		public static int Random(int count)
		{
			return m_random.Next(count);
		}
		#endregion

		#region Range
		public static bool InUpdateRange(Mobile from, Mobile to)
		{
			return InRange(from.Location, to.Location, World.GLOBAL_MAX_UPDATE_RANGE);
		}

		public static bool InUpdateRange(Point location, Point point)
		{
			return InRange(location, point, World.GLOBAL_MAX_UPDATE_RANGE);
		}

		public static bool InRange(Mobile from, Mobile to, int range)
		{
			return InRange(from.Location, to.Location, range);
		}

		public static bool InRange(Point from, Point to, int range)
		{
			return to.X > from.X - range && to.X < from.X + range
					&& to.Y > from.Y - range && to.Y < from.Y + range;
		}
		#endregion

		public static bool NumberBetween(double num, double bound1, double bound2, double allowance)
		{
			if (bound1 > bound2)
			{
				var swap = bound1;
				bound1 = bound2;
				bound2 = swap;
			}

			return (num < bound2 + allowance && num > bound1 - allowance);
		}

		public static void FixPoints(ref Point top, ref Point bottom)
		{
			if (bottom.X < top.X)
			{
				var swap = top.X;
				top.X = bottom.X;
				bottom.X = swap;
			}

			if (bottom.Y < top.Y)
			{
				var swap = top.Y;
				top.Y = bottom.Y;
				bottom.Y = swap;
			}
		}

		public static Direction GetDisplayOrientation(Point from, Point to)
		{
			var dX = to.X - from.X;
			var dY = to.Y - from.Y;

			if (Math.Abs(dX) > 3 * Math.Abs(dY))
			{
				return dX > 0 ? Direction.East : Direction.West;
			}
			else if (Math.Abs(dY) > 3 * Math.Abs(dX))
			{
				return dY > 0 ? Direction.South : Direction.North;
			}

			var direction = default(Direction);

			if (dX > 0)
			{
				direction = direction | Direction.East;
			}
			else if (dX < 0)
			{
				direction = direction | Direction.West;
			}

			if (dY > 0)
			{
				direction = direction | Direction.South;
			}
			else if (dY < 0)
			{
				direction = direction | Direction.North;
			}

			return direction;
		}

	}
}
