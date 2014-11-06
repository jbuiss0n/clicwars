using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServer
{
	public struct Serial : IComparable, IComparable<Serial>
	{
		private static Serial m_lastMobile = Zero;
		private static Serial m_lastItem = 0x40000000;

		public static Serial LastMobile { get { return m_lastMobile; } }
		public static Serial LastItem { get { return m_lastItem; } }

		public static readonly Serial MinusOne = new Serial(-1);
		public static readonly Serial Zero = new Serial(0);

		public static Serial NewMobile
		{
			get
			{
				while (World.FindMobile(m_lastMobile = (m_lastMobile + 1)) != null) ;

				return m_lastMobile;
			}
		}

		private int m_serial;

		private Serial(int serial)
		{
			m_serial = serial;
		}

		public int Value
		{
			get
			{
				return m_serial;
			}
		}

		public bool IsMobile
		{
			get
			{
				return (m_serial > 0 && m_serial < 0x40000000);
			}
		}

		public bool IsItem
		{
			get
			{
				return (m_serial >= 0x40000000 && m_serial <= 0x7FFFFFFF);
			}
		}

		public bool IsValid
		{
			get
			{
				return (m_serial > 0);
			}
		}

		public override int GetHashCode()
		{
			return m_serial;
		}

		public int CompareTo(Serial other)
		{
			return m_serial.CompareTo(other.m_serial);
		}

		public int CompareTo(object other)
		{
			if (other is Serial)
				return this.CompareTo((Serial)other);
			else
				return -1;
		}

		public override bool Equals(object o)
		{
			if (o == null || !(o is Serial)) return false;

			return ((Serial)o).m_serial == m_serial;
		}

		public static bool operator ==(Serial l, Serial r)
		{
			return l.m_serial == r.m_serial;
		}

		public static bool operator !=(Serial l, Serial r)
		{
			return l.m_serial != r.m_serial;
		}

		public static bool operator >(Serial l, Serial r)
		{
			return l.m_serial > r.m_serial;
		}

		public static bool operator <(Serial l, Serial r)
		{
			return l.m_serial < r.m_serial;
		}

		public static bool operator >=(Serial l, Serial r)
		{
			return l.m_serial >= r.m_serial;
		}

		public static bool operator <=(Serial l, Serial r)
		{
			return l.m_serial <= r.m_serial;
		}

		public override string ToString()
		{
			return String.Format("0x{0:X8}", m_serial);
		}

		public static implicit operator int(Serial a)
		{
			return a.m_serial;
		}

		public static implicit operator Serial(int a)
		{
			return new Serial(a);
		}
	}
}