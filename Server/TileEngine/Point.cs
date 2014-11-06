using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
	public struct Point : IComparable, IComparable<Point>
	{
		public static readonly Point Zero = new Point(0, 0);

		public double X;

		public double Y;

		public Point(double x, double y)
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return String.Format("({0}, {1})", X, Y);
		}

		public static Point Parse(string value)
		{
			var start = value.IndexOf('(');
			var end = value.IndexOf(',', start + 1);

			var param1 = value.Substring(start + 1, end - (start + 1)).Trim();

			start = end;
			end = value.IndexOf(')', start + 1);

			var param2 = value.Substring(start + 1, end - (start + 1)).Trim();

			return new Point(Convert.ToDouble(param1), Convert.ToDouble(param2));
		}

		public int CompareTo(Point other)
		{
			int v = (X.CompareTo(other.X));

			if (v == 0)
				v = (Y.CompareTo(other.Y));

			return v;
		}

		public int CompareTo(object other)
		{
			if (other is Point)
				return this.CompareTo((Point)other);
			else
				return -1;
		}

		public override bool Equals(object o)
		{
			if (o == null || !(o is Point)) return false;

			Point p = (Point)o;

			return X == p.X && Y == p.Y;
		}

		public static bool operator ==(Point l, Point r)
		{
			return l.X == r.X && l.Y == r.Y;
		}

		public static bool operator !=(Point l, Point r)
		{
			return l.X != r.X || l.Y != r.Y;
		}

		public static bool operator >(Point l, Point r)
		{
			return l.X > r.X && l.Y > r.Y;
		}

		public static bool operator <(Point l, Point r)
		{
			return l.X < r.X && l.Y < r.Y;
		}

		public static bool operator >=(Point l, Point r)
		{
			return l.X >= r.X && l.Y >= r.Y;
		}

		public static bool operator <=(Point l, Point r)
		{
			return l.X <= r.X && l.Y <= r.Y;
		}

		public static Point operator +(Point l, Point r)
		{
			return new Point(l.X + r.X, l.Y + l.Y);
		}

		public static Point operator -(Point l, Point r)
		{
			return new Point(l.X - r.X, l.Y - l.Y);
		}
	}
}