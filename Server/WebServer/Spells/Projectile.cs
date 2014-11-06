using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;
using WebServer.Network.Packet;

namespace WebServer.Spells
{
	public class Projectile : Spell
	{
		private EventTimer m_timer;

		public override SpellType Type { get { return SpellType.Projectile; } }

		public override int ManaCost { get { return 10; } }

		public override TimeSpan Delay { get { return TimeSpan.FromSeconds(0.5); } }

		public virtual int Model { get { return -1; } }

		public virtual int Width { get { return 32; } }

		public virtual Point Location { get; protected set; }

		public virtual Point Target { get; protected set; }

		public virtual int Speed { get; protected set; }

		public virtual int Range { get; protected set; }

		public Projectile(Mobile caster, Point target, int speed)
			: base(caster)
		{
			Location = Caster.Location;
			Target = target;
			Speed = speed;
		}

		public override void OnCast()
		{
			base.OnCast();

			var direction = Utility.GetDisplayOrientation(Location, Target);

			double x = Caster.Location.X;
			double y = Caster.Location.Y;

			var dX = Target.X - x;
			var dY = Target.Y - y;

			var tX = Math.Abs(Math.Ceiling(dX / Speed));
			var tY = Math.Abs(Math.Ceiling(dY / Speed));

			var tMax = Math.Max(tX, tY);

			var rX = dX / tMax;
			var rY = dY / tMax;

			var total = (int)tMax;

			m_timer = new EventTimer(World.GLOBAL_UPDATE_TIME, World.GLOBAL_UPDATE_TIME, () => OnTick(ref x, ref y, rX, rY));
		}

		private void OnTick(ref double x, ref double y, double rX, double rY)
		{
			x += rX;
			y += rY;

			SetLocation(Caster.Map.GetPointInRange(x, y));

			if (!Caster.Map.CanShootThrough(Location))
			{
				Delete();
				return;
			}

			var mobiles = Caster.Map.GetMobilesInRange(Location, Width);

			foreach (var mobile in mobiles)
			{
				if (mobile != Caster)
				{
					OnHit(mobile);
					Delete();
					return;
				}
			}
		}

		private void OnHit(Mobile mobile)
		{
			throw new NotImplementedException();
		}

		public override void OnDelete()
		{
			if (m_timer != null && m_timer.Running)
				m_timer.Stop();

			var mobiles = Caster.Map.GetMobilesInRange(Location, World.GLOBAL_MAX_UPDATE_RANGE);

			foreach (var mobile in mobiles)
			{
				var player = mobile as PlayerMobile;
				if (player != null && player.Client != null)
				{
					
				}
			}

			base.OnDelete();
		}

		private void SetLocation(Point newLocation)
		{
			if (newLocation == Location)
				return;

			var oldLocation = Location;
			Location = newLocation;

			//send a removing message to everyone who can no longer see us
			var objectInOldRange = Caster.Map.GetObjectsInRange(oldLocation, World.GLOBAL_MAX_UPDATE_RANGE);
			foreach (var obj in objectInOldRange)
			{
				if (obj != this && obj is Mobile)
				{
					Mobile mobile = (Mobile)obj;

					bool inRange = Utility.InUpdateRange(newLocation, mobile.Location);

					if (!inRange)
					{
						mobile.OnRemoving(this, oldLocation);
					}
				}
			}

			//send an incomming message to everyone who can now see us, and a moving message to everyone who already see us
			var objectInRange = Caster.Map.GetObjectsInRange(newLocation, World.GLOBAL_MAX_UPDATE_RANGE);
			foreach (var obj in objectInRange)
			{
				if (obj != this && obj is Mobile)
				{
					Mobile mobile = (Mobile)obj;

					bool inOldRange = Utility.InUpdateRange(oldLocation, mobile.Location);

					if (!inOldRange)
					{
						//Send create
					}
					else
					{
						//Send upadte
					}
				}
			}
		}
	}
}
