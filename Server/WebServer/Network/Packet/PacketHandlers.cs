using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServer.Accounting;

namespace WebServer.Network.Packet
{
	public class PacketHandlers
	{
		private static PacketHandler[] m_handlers;

		public static PacketHandler[] Handlers
		{
			get { return m_handlers; }
		}

		static PacketHandlers()
		{
			m_handlers = new PacketHandler[0x1000];

			Register(PacketIds.LoginRequest, false, typeof(LoginRequestPacket), new OnPacketReceive(LoginRequest));
			Register(PacketIds.MovementRequest, false, typeof(MovementRequestPacket), new OnPacketReceive(MovementRequest));
			Register(PacketIds.FireballRequest, false, typeof(FireballRequestPacket), new OnPacketReceive(FireballRequest));
			Register(PacketIds.PlayerRespawnRequest, false, typeof(RespawnRequestPacket), new OnPacketReceive(RespawnRequest));

			Register(PacketIds.ChatMessage, false, typeof(ChatMessagePacket), new OnPacketReceive(ChatMessage));
		}

		private static void Register(int packetID, bool ingame, Type type, OnPacketReceive onReceive)
		{
			m_handlers[packetID] = new PacketHandler(packetID, ingame, type, onReceive);
		}

		public static PacketHandler GetHandler(int packetID)
		{
			return m_handlers[packetID];
		}

		private static void FireballRequest(Client client, Packet packet)
		{
			var fireballPacket = (FireballRequestPacket)packet;

			if (client.Player == null)
			{
				client.Dispose();
				return;
			}

			client.Player.CastFireball((int)Math.Round(fireballPacket.X), (int)Math.Round(fireballPacket.Y));
		}

		private static void MovementRequest(Client client, Packet packet)
		{
			var movementPacket = (MovementRequestPacket)packet;

			if (client.Player == null)
			{
				client.Dispose();
				return;
			}

			if (!client.Player.Move((Direction)movementPacket.Direction))
				client.Send(MovementRejectPacket.Acquire(client.Player));
		}

		private static void LoginRequest(Client client, Packet packet)
		{
			var loginPacket = (LoginRequestPacket)packet;
			var account = AccountManager.GetAccount(loginPacket.Username);

			if (account.GameToken == null
				|| account.GameToken.Value != loginPacket.Token
				|| account.GameToken.CreatedDate >= DateTime.Now + GameToken.EXPIRATION_TIME)
				return;

			var mobile = World.FindMobile(account.GameToken.Serial);
			var player = mobile as PlayerMobile;

			account.GameToken = null;

			if (player == null || player.Account != account)
				return;

			player.Client = client;
			AddPlayerToWorld(client);
		}

		private static void RespawnRequest(Client client, Packet packet)
		{
			if (client.Player.Status != PlayerStatus.Dead)
				return;

			client.Player.Resurect();
			AddPlayerToWorld(client);
		}

		private static void ChatMessage(Client client, Packet packet)
		{
			if (client.Player == null)
				return;

			if (client.Player.Map == null)
				return;

			//FIXME : unescape / check the content
			client.Player.Map.Broadcast(packet);
		}

		private static void AddPlayerToWorld(Client client)
		{
			//if (client.Player.Map == null)
			{
				client.Player.SetMap(World.FindMap(0)); //FIXME : multimap
			}

			client.Send(MapChangePacket.Acquire(client.Player.Map));
			client.Send(MobileIncomingPacket.Acquire(client.Player));
			client.Player.SendEverything();
			client.Player.SendIncomingPacket();
		}
	}
}
