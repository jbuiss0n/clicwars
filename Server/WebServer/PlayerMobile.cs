using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServer.Accounting;
using WebServer.Network;
using WebServer.Persistence;
using TileEngine;
using WebServer.Network.Packet;
using WebServer.Spells;

namespace WebServer
{
	public enum PlayerStatus
	{
		Alive = 0x01,
		Dead = 0x02,
	}

	public class PlayerMobile : Mobile
	{
		protected LogoutTimer m_logoutTimer;

		protected Client m_client;

		public Account Account { get; set; }

		public DateTime SessionStart { get; set; }

		public DateTime CreationDate { get; protected set; }

		public int Kills { get; protected set; }

		public int Deaths { get; protected set; }

		public Client Client
		{
			get
			{
				if (m_client != null && m_client.Socket == null)
					m_client = null;

				return m_client;
			}
			set
			{
				m_client = value;
				OnClientChange();
			}
		}

		public PlayerStatus Status { get; protected set; }

		public PlayerMobile(Serial serial)
			: base(serial)
		{
			Width = 32;
			Height = 32;
		}

		private PlayerMobile()
			: base()
		{
			Width = 32;
			Height = 32;
			CreationDate = DateTime.Now;
		}

		public override void OnIncomming(Mobile mobile, Point oldLocation)
		{
			base.OnIncomming(mobile, oldLocation);

			SendPacket(MobileIncomingPacket.Acquire(mobile));
		}

		public override void OnRemoving(Mobile mobile, Point oldLocation)
		{
			base.OnRemoving(mobile, oldLocation);

			SendPacket(MobileRemovingPacket.Acquire(mobile));
		}

		public override void OnMovement(Mobile mobile, Point oldLocation)
		{
			base.OnMovement(mobile, oldLocation);

			SendPacket(MobileMovingPacket.Acquire(mobile));
		}

		public override void OnIncomming(Projectile projectile, Point oldLocation)
		{
			base.OnIncomming(projectile, oldLocation);

			//Client.Send(MobileIncomingPacket.Acquire(mobile));
		}

		public override void OnRemoving(Projectile projectile, Point oldLocation)
		{
			base.OnRemoving(projectile, oldLocation);

			//Client.Send(MobileRemovingPacket.Acquire(mobile));
		}

		public override void OnMovement(Projectile projectile, Point oldLocation)
		{
			base.OnMovement(projectile, oldLocation);

			//Client.Send(MobileMovingPacket.Acquire(mobile));
		}

		public override void OnDeath(Mobile killer)
		{
			base.OnDeath(killer);
			Deaths++;
			Status = PlayerStatus.Dead;
			SendPacket(PlayerDeathPacket.Acquire(this));
			EventSink.InvokePlayerDeath(new PlayerDeathEventArgs(this, killer));
		}

		public override void OnKill(Mobile mobile)
		{
			base.OnKill(mobile);
			Kills++;
		}

		public override void Damage(int amount, Mobile from, bool crit)
		{
			base.Damage(amount, from, crit);

			SendStatus();
		}

		public void Resurect()
		{
			Hits = 100;
			Mana = 100;

			Map.OnEnter(this);

			SetupRegenTimers();
		}

		public void SendEverything()
		{
			if (Map != null && Client != null)
			{
				var objectInRange = Map.GetObjectsInRange(Location, World.GLOBAL_MAX_UPDATE_RANGE);

				foreach (var obj in objectInRange)
				{
					if (obj != this && obj is Mobile)
					{
						Mobile mobile = (Mobile)obj;

						if (CanSee(mobile) && Utility.InUpdateRange(Location, mobile.Location))
						{
							SendPacket(MobileIncomingPacket.Acquire(mobile));
						}

					}
				}
			}
		}

		public void CastFireball(int x, int y)
		{
			var target = new Point(x, y);

			if (!CheckCastFireball(target))
				return;

			Mana -= Fireball.ManaCost;
			LastCast += Fireball.CastDelay;
			new Fireball(this, target, 5).Cast();

			SendStatus();
		}

		public void SendPacket(Packet packet)
		{
			if (Client != null)
				Client.Send(packet);
		}

		protected override void HitRegen()
		{
			base.HitRegen();

			SendStatus();
		}

		protected override void ManaRegen()
		{
			base.ManaRegen();

			SendStatus();
		}

		private void OnClientChange()
		{
			if (m_client == null)
			{
				OnDisconnected();

				if (m_logoutTimer == null)
					m_logoutTimer = new LogoutTimer(this);
				else
					m_logoutTimer.Stop();

				m_logoutTimer.Delay = LogoutDelay;
				m_logoutTimer.Start();
			}
			else
			{
				m_client.Player = this;

				OnConnected();

				if (m_logoutTimer != null)
					m_logoutTimer.Stop();

				m_logoutTimer = null;
			}
		}

		private void OnConnected()
		{
			if (Map != null)
				Map.OnEnter(this);

			SetupRegenTimers();
			SendIncomingPacket();
			EventSink.InvokeConnected(new ConnectedEventArgs(this));
		}

		private void OnDisconnected()
		{
			if (Map != null)
				Map.OnLeave(this);

			KillRegenTimers();
			SendRemovingPacket();
			EventSink.InvokeDisconnected(new DisconnectedEventArgs(this));
		}

		private void SendStatus()
		{
			SendPacket(PlayerStatusPacket.Acquire(this));
		}

		private bool CheckCastFireball(Point target)
		{
			if (Mana < Fireball.ManaCost)
				return false;

			if (LastCast + Fireball.CastDelay > DateTime.Now)
				return false;

			return true;
		}

		public static PlayerMobile Create(string name, int body)
		{
			var player = new PlayerMobile
			{
				Name = name,
				Body = body,
				Hits = 100,
				Mana = 100,

				//TODO : character customization
			};

			return player;
		}

		#region Serialization
		private const int VERSION = 1;

		public override void Serialize(BinaryWriter writer)
		{
			base.Serialize(writer);

			writer.Write(VERSION);

			writer.Write(CreationDate);
			writer.Write(Kills);
			writer.Write(Deaths);
		}

		public override void Deserialize(BinaryReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt32();

			switch (version)
			{
				case 1:
					{
						CreationDate = reader.ReadDateTime();
						Kills = reader.ReadInt32();
						Deaths = reader.ReadInt32();
						break;
					}
			}
		}
		#endregion
	}
}
