using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;
using WebServer.Network;
using WebServer.Network.Packet;
using WebServer.Persistence;
using DungeonResources;
using WebServer.Spells;

namespace WebServer
{
	[Flags]
	public enum Direction
	{
		North = 0x1,
		East = 0x2,
		South = 0x4,
		West = 0x8
	}

	public class Mobile : IEntity
	{
		public static TimeSpan MovementDelay = TimeSpan.FromMilliseconds(15);
		public static TimeSpan LogoutDelay = TimeSpan.FromMinutes(5.0);

		protected Timer m_hitsRegenTimer;
		protected Timer m_manaRegenTimer;

		public Point Location { get; protected set; }

		public Direction Direction { get; protected set; }

		public DateTime LastMove { get; protected set; }

		public DateTime LastCast { get; protected set; }

		public Serial Serial { get; protected set; }

		public Map Map { get; protected set; }

		public string Name { get; protected set; }

		public int Body { get; protected set; }

		public int Width { get; protected set; }

		public int Height { get; protected set; }

		public bool Deleted { get; protected set; }

		public double Hits { get; set; }

		public double Mana { get; set; }

		public virtual double ManaRegenRate { get { return 4; } }

		public virtual double HitsRegenRate { get { return 1; } }

		public virtual double MovementSpeed { get { return 5.0; } }

		public virtual double HitsMax { get { return 100; } }

		public virtual double ManaMax { get { return 100; } }

		public Mobile()
			: this(Serial.NewMobile)
		{
		}

		public Mobile(Serial serial)
		{
			Serial = serial;
			World.AddMobile(this);
			LastMove = DateTime.Now;
		}

		public virtual bool Move(Direction direction)
		{
			if (Deleted)
				return false;

			if (Map == null)
				return false;

			if (!CanMove())
				return false;

			Point newLocation = Location;
			Point oldLocation = Location;

			if (Direction == direction)
			{
				if (direction == Direction.North)
				{
					newLocation.Y -= MovementSpeed;
				}
				else if (direction == Direction.East)
				{
					newLocation.X += MovementSpeed;
				}
				else if (direction == Direction.South)
				{
					newLocation.Y += MovementSpeed;
				}
				else if (direction == Direction.West)
				{
					newLocation.X -= MovementSpeed;
				}
				else
				{
					var speed = Utility.COS45 * MovementSpeed;

					if (direction.HasFlag(Direction.North))
						newLocation.Y -= speed;

					if (direction.HasFlag(Direction.East))
						newLocation.X += speed;

					if (direction.HasFlag(Direction.South))
						newLocation.Y += speed;

					if (direction.HasFlag(Direction.West))
						newLocation.X -= speed;
				}

				if (!CheckMovement(newLocation))
					return false;

				if (!Map.CanMove(this, newLocation))
					return false;

				LastMove = DateTime.Now;
			}

			SetLocation(newLocation);
			SetDirection(direction);

			return true;
		}

		public virtual void SetMap(Map map)
		{
			if (Map != null)
				Map.OnLeave(this);

			Map = map;

			if (Map == null)
				return;

			Map.OnEnter(this);
			SetLocation(map.GetStartLocation());
		}

		public virtual void SetDirection(Direction direction)
		{
			if (Direction == direction)
				return;

			Direction = direction;

			//send a moving message to everyone who already see us to update our location
			var objectInRange = Map.GetObjectsInRange(Location, World.GLOBAL_MAX_UPDATE_RANGE);
			foreach (var obj in objectInRange)
			{
				if (obj != this && obj is Mobile)
				{
					Mobile mobile = (Mobile)obj;

					if (mobile.CanSee(this))
					{
						mobile.OnMovement(this, Location);
					}
				}
			}
		}

		public virtual void SetLocation(Point newLocation)
		{
			if (newLocation == Location)
				return;

			var oldLocation = Location;
			Location = newLocation;

			Map.OnMove(oldLocation, this);
			OnMovement(this, oldLocation);

			//send a removing message to everyone who can no longer see us
			var objectInOldRange = Map.GetObjectsInRange(oldLocation, World.GLOBAL_MAX_UPDATE_RANGE + 50);// FIXME : dirty way to correct the issue when a fireball is not removed
			foreach (var obj in objectInOldRange)
			{
				if (obj != this && obj is Mobile)
				{
					Mobile mobile = (Mobile)obj;

					bool inRange = Utility.InUpdateRange(newLocation, mobile.Location);

					if (!inRange)
					{
						mobile.OnRemoving(this, oldLocation);
						OnRemoving(mobile, oldLocation);
					}
				}
			}

			//send an incomming message to everyone who can now see us, and a moving message to everyone who already see us
			var objectInRange = Map.GetObjectsInRange(newLocation, World.GLOBAL_MAX_UPDATE_RANGE);
			foreach (var obj in objectInRange)
			{
				if (obj != this && obj is Mobile)
				{
					Mobile mobile = (Mobile)obj;

					bool inOldRange = Utility.InUpdateRange(oldLocation, mobile.Location);

					if (!inOldRange && mobile.CanSee(this))
					{
						mobile.OnIncomming(this, oldLocation);
						OnIncomming(mobile, oldLocation);
					}
					else if (mobile.CanSee(this))
					{
						mobile.OnMovement(this, oldLocation);
						//OnMovement(mobile, oldLocation); WTF : Why are we sending to this a movement from mobile, IT IS ME WHO IS MOVING !
					}
				}
			}
		}

		public virtual bool CheckMovement(Point newLocation)
		{
			return LastMove + MovementDelay < DateTime.Now;
		}

		public virtual bool CanSee(Mobile mobile)
		{
			return true;
		}

		public virtual bool CanSee(Point location)
		{
			if (!Map.LineOfSight(Location, location))
				return false;

			return true;
		}

		public virtual bool CanMove()
		{
			return true;
		}

		public virtual void SendIncomingPacket()
		{
			if (Deleted || Map == null)
				return;

			var mobiles = Map.GetMobilesInRange(Location, World.GLOBAL_MAX_UPDATE_RANGE);

			foreach (var mobile in mobiles)
			{
				if (mobile != this && mobile.CanSee(this))
				{
					mobile.OnIncomming(this, Location);
				}
			}
		}

		public virtual void SendRemovingPacket()
		{
			if (Deleted || Map == null)
				return;

			var mobiles = Map.GetMobilesInRange(Location, World.GLOBAL_MAX_UPDATE_RANGE);

			foreach (var mobile in mobiles)
			{
				if (mobile != this && mobile.CanSee(this))
				{
					mobile.OnRemoving(this, Location);
				}
			}
		}

		public virtual void SendToAll(Packet packet)
		{
			if (Deleted || Map == null)
				return;

			var objectInRange = Map.GetObjectsInRange(Location, World.GLOBAL_MAX_UPDATE_RANGE);

			foreach (var obj in objectInRange)
			{
				if (obj is PlayerMobile)
				{
					PlayerMobile player = (PlayerMobile)obj;

					if (obj == this || player.CanSee(this))
					{
						player.SendPacket(packet);
					}
				}
			}
		}

		public virtual void Delete()
		{
			OnDelete();
			Deleted = true;

			if (Map != null)
			{
				Map.OnLeave(this);
				Map = null;
			}

			World.RemoveMobile(this);
		}

		public virtual void OnDelete()
		{
			SendRemovingPacket();
			KillRegenTimers();
		}

		public virtual void OnRemoving(Mobile mobile, Point oldLocation)
		{
		}

		public virtual void OnRemoving(Projectile projectile, Point oldLocation)
		{
		}

		public virtual void OnIncomming(Mobile mobile, Point oldLocation)
		{
		}

		public virtual void OnIncomming(Projectile projectile, Point oldLocation)
		{
		}

		public virtual void OnMovement(Mobile projectile, Point oldLocation)
		{
		}

		public virtual void OnMovement(Projectile projectile, Point oldLocation)
		{
		}

		public virtual void OnDamage(int amount, Mobile from, bool critical)
		{
			if (amount <= 0)
				return;
		}

		public virtual void OnDeath(Mobile killer)
		{
			Delete();
		}

		public virtual bool OnBeforeDeath()
		{
			return true;
		}

		public virtual void Damage(int amount, Mobile from, bool crit)
		{
			Hits = Math.Max(Hits - amount, 0);

			if (Hits == 0)
				Die(from);
		}

		public virtual void Die(Mobile killer)
		{
			if (Deleted || !Map.OnBeforeDeath(this, killer) || !OnBeforeDeath())
				return;

			Hits = 0;

			OnDeath(killer);
			killer.OnKill(this);
			Map.OnDeath(this, killer);
			SendRemovingPacket();
		}

		public virtual void OnKill(Mobile mobile)
		{
		}

		public virtual bool InLineOfSight(Mobile target)
		{
			if (Map == null)
				return false;

			if (target == this)
				return true;

			return Map.LineOfSight(this, target);
		}

		protected virtual void SetupRegenTimers()
		{
			m_hitsRegenTimer = new RegenTimer(HitRegen, true);
			m_manaRegenTimer = new RegenTimer(ManaRegen, true);
		}

		protected virtual void KillRegenTimers()
		{
			if (m_hitsRegenTimer != null)
			{
				if (m_hitsRegenTimer.Running)
					m_hitsRegenTimer.Stop();
				m_hitsRegenTimer = null;
			}

			if (m_manaRegenTimer != null)
			{
				if (m_manaRegenTimer.Running)
					m_manaRegenTimer.Stop();
				m_manaRegenTimer = null;
			}
		}

		protected virtual void ManaRegen()
		{
			if (Mana >= 100)
				return;

			var rate = ManaRegenRate;
			Mana += rate;

			Mana = Math.Min(100, Mana);
		}

		protected virtual void HitRegen()
		{
			if (Hits >= 100)
				return;

			var rate = HitsRegenRate;
			Hits += rate;

			Hits = Math.Min(100, Hits);
		}

		#region CompareTo
		public int CompareTo(IEntity other)
		{
			if (other == null)
				return -1;

			return Serial.CompareTo(other.Serial);
		}

		public int CompareTo(Mobile other)
		{
			return CompareTo((IEntity)other);
		}

		public int CompareTo(object other)
		{
			if (other == null || other is IEntity)
				return CompareTo((IEntity)other);

			return -1;
		}
		#endregion

		#region Serialization
		private const int VERSION = 1;

		public virtual void Serialize(BinaryWriter writer)
		{
			writer.Write(VERSION);

			writer.Write(Hits);
			writer.Write(Body);
			writer.Write(Location);
			writer.Write(Name);
			writer.Write(Map);
		}

		public virtual void Deserialize(BinaryReader reader)
		{
			int version = reader.ReadInt32();

			switch (version)
			{
				case 1:
					{
						Hits = reader.ReadDouble();
						Body = reader.ReadInt32();
						Location = reader.ReadPoint();
						Name = reader.ReadString();
						Map = reader.ReadMap();
						break;
					}
			}
		}
		#endregion

		protected class RegenTimer : Timer
		{
			protected Action m_onTick;

			public RegenTimer(Action onTick, bool start = false)
				: base(TimeSpan.Zero, TimeSpan.FromSeconds(1))
			{
				m_onTick = onTick;

				if (start)
					Start();
			}

			protected override void OnTick()
			{
				m_onTick();
			}
		}
	}
}