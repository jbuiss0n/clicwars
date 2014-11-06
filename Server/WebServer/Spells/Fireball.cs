using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;
using WebServer.Network.Packet;

namespace WebServer.Spells
{
	public class Fireball : Mobile
	{
		public static int ManaCost = 5;
		public static TimeSpan CastDelay = TimeSpan.FromSeconds(0.5);
		public static int FireballDamageMin = 20;
		public static int FireballDamageMax = 25;
		private Mobile m_owner;
		private FireballTimer m_timer;

		public Fireball(Mobile mobile, Point target, int speed)
			: base(Serial.NewMobile)
		{
			Body = -1;
			Width = 32;
			Height = 32;
			m_owner = mobile;
			Location = m_owner.Location;
			Map = m_owner.Map;

			Direction = Utility.GetDisplayOrientation(Location, target);

			m_timer = new FireballTimer(this, m_owner, target, speed);
		}

		public void Hit(PlayerMobile player)
		{
			var damage = Utility.RandomMinMax(FireballDamageMin, FireballDamageMax);

			player.Damage(damage, m_owner, false);
		}

		public void Cast()
		{
			SendIncomingPacket();

			m_timer.Start();
		}

		public override void Delete()
		{
			if (m_timer != null && m_timer.Running)
			{
				m_timer.Delete();
			}

			var mobiles = Map.GetMobilesInRange(Location, World.GLOBAL_MAX_UPDATE_RANGE);

			foreach (var mobile in mobiles)
			{
				var player = mobile as PlayerMobile;
				if (player != null && player.Client != null)
				{
					player.SendPacket(EffectPacket.Acquire(EffectIds.FireballExplosion, Location.X, Location.Y));
				}
			}

			base.Delete();
		}

		private class FireballTimer : Timer
		{
			private int m_count = 0;
			private int m_total = 0;
			private int m_speed = 0;

			private double m_x = 0;
			private double m_y = 0;

			private double m_rX = 0;
			private double m_rY = 0;

			private Fireball m_parent;
			private Mobile m_caster;
			private Point m_target;

			public FireballTimer(Fireball fireball, Mobile mobile, Point target, int speed)
				: base(TimeSpan.FromMilliseconds(15), TimeSpan.FromMilliseconds(15), 0)
			{
				m_parent = fireball;
				m_caster = mobile;
				m_target = target;
				m_speed = speed;

				m_x = m_caster.Location.X;
				m_y = m_caster.Location.Y;

				var dX = m_target.X - m_x;
				var dY = m_target.Y - m_y;

				if (dX == 0) dX = 1;
				if (dY == 0) dY = 1;

				var tX = Math.Abs(Math.Ceiling(dX / (double)speed));
				var tY = Math.Abs(Math.Ceiling(dY / (double)speed));

				var tMax = Math.Max(tX, tY);

				m_rX = dX / tMax;
				m_rY = dY / tMax;

				m_total = (int)tMax;
			}

			protected override void OnTick()
			{
				m_x += m_rX;
				m_y += m_rY;

				var point = new Point(Math.Max(0, (int)Math.Round(m_x)), Math.Min((int)Math.Round(m_y), m_parent.Map.TotalWidth));

				m_parent.SetLocation(point);

				if (!m_parent.Map.CanShootThrough(point))
				{
					Delete();
					return;
				}

				var mobiles = m_parent.Map.GetMobilesInRange(point, m_parent.Width);

				foreach (var mobile in mobiles)
				{
					var player = mobile as PlayerMobile;
					if (player != null && player.Client != null && player != m_caster)
					{
						m_parent.Hit(player);
						Delete();
						return;
					}
				}

				m_count++;

				if (m_count > m_total)
				{
					Delete();
				}
			}

			public void Delete()
			{
				Stop();
				m_parent.Delete();
			}
		}
	}
}
