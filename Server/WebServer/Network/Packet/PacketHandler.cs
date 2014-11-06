using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServer.Network.Packet
{
	public delegate void OnPacketReceive(Client client, Packet packet);

	public class PacketIds
	{
		public const int LoginRequest = 0x01;

		public const int MapChange = 0x10;

		public const int MovementRequest = 0x20;
		public const int MovementReject = 0x21;

		public const int MobileIncoming = 0x30;
		public const int MobileRemoving = 0x31;
		public const int MobileMoving = 0x32;

		public const int PlayerStatus = 0x40;
		public const int PlayerDeath = 0x41;
		public const int PlayerRespawnRequest = 0x42;

		public const int FireballRequest = 0x50;

		public const int Effect = 0x60;
		public const int ChatMessage = 0x61;

		public const int RespawnRequest = 0x71;

	}

	public class PacketHandler
	{
		private int m_packetID;
		private bool m_ingame;
		private Type m_type;
		private OnPacketReceive m_onReceive;

		public int PacketID
		{
			get
			{
				return m_packetID;
			}
		}

		public bool Ingame
		{
			get
			{
				return m_ingame;
			}
		}

		public Type Type
		{
			get
			{
				return m_type;
			}
		}

		public OnPacketReceive OnReceive
		{
			get
			{
				return m_onReceive;
			}
		}

		public PacketHandler(int packetID, bool ingame, Type type, OnPacketReceive onReceive)
		{
			m_packetID = packetID;
			m_ingame = ingame;
			m_type = type;
			m_onReceive = onReceive;
		}
	}
}
