using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServer.Network;
using WebServer.Network.Packet;

namespace WebServer.Engines.ChatEngine
{
	public static class ChatManager
	{
		public static void Configure()
		{
			EventSink.PlayerDeath += EventSink_PlayerDeath;
			EventSink.Connected += EventSink_Connected;
			EventSink.Disconnected += EventSink_Disconnected;
		}

		private static void EventSink_PlayerDeath(PlayerDeathEventArgs e)
		{
			if (e.Player.Map == null)
				return;

			if (e.Killer != null)
				e.Player.Map.Broadcast(ChatMessagePacket.Acquire(ChatMessageType.Map, null, String.Format("{0} killed {1}", e.Killer.Name, e.Player.Name)));
			else
				e.Player.Map.Broadcast(ChatMessagePacket.Acquire(ChatMessageType.Map, null, String.Format("{0} is dead", e.Player.Name)));
		}

		private static void EventSink_Disconnected(DisconnectedEventArgs e)
		{
			World.Broadcast(ChatMessagePacket.Acquire(ChatMessageType.World, null, String.Format("{0} has leave the game", e.Player.Name)));
		}

		private static void EventSink_Connected(ConnectedEventArgs e)
		{
			World.Broadcast(ChatMessagePacket.Acquire(ChatMessageType.World, null, String.Format("{0} has join the game", e.Player.Name)));
		}
	}
}
