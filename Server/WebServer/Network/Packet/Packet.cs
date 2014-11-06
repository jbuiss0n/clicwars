using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using TileEngine;
using WebServer.Engines.ChatEngine;

namespace WebServer.Network.Packet
{
	public abstract class Packet
	{
		public int Id { get; set; }

		public Packet(int id)
		{
			Id = id;
		}
	}

	#region LOGIN/LOGOUT PACKET
	public class LoginRequestPacket : Packet
	{
		public string Username { get; set; }

		public string Token { get; set; }

		public LoginRequestPacket()
			: base(PacketIds.LoginRequest)
		{

		}

		public static LoginRequestPacket Acquire(string username, string token)
		{
			return new LoginRequestPacket { Username = username, Token = token };
		}
	}
	#endregion

	#region MAP PACKET
	public class MapChangePacket : Packet
	{
		public int MapId { get; set; }

		public MapChangePacket()
			: base(PacketIds.MapChange)
		{

		}

		public static MapChangePacket Acquire(Map map)
		{
			return new MapChangePacket { MapId = map.Id };
		}
	}
	#endregion

	#region MOBILE PACKET
	public class MovementRequestPacket : Packet
	{
		public int Direction { get; set; }

		public MovementRequestPacket()
			: base(PacketIds.MovementRequest)
		{
		}

		public static MovementRequestPacket Acquire(Mobile mobile)
		{
			return new MovementRequestPacket { Direction = (int)mobile.Direction };
		}
	}

	public class MovementRejectPacket : Packet
	{
		public double X { get; set; }

		public double Y { get; set; }

		public int Direction { get; set; }

		public MovementRejectPacket()
			: base(PacketIds.MovementReject)
		{

		}

		public static MovementRejectPacket Acquire(Mobile mobile)
		{
			return new MovementRejectPacket { X = mobile.Location.X, Y = mobile.Location.Y, Direction = (int)mobile.Direction };
		}
	}

	public class MobileIncomingPacket : Packet
	{
		public int Serial { get; set; }

		public int Body { get; set; }

		public double X { get; set; }

		public double Y { get; set; }

		public int Direction { get; set; }

		public MobileIncomingPacket()
			: base(PacketIds.MobileIncoming)
		{
		}

		public static MobileIncomingPacket Acquire(Mobile mobile)
		{
			return new MobileIncomingPacket { Serial = mobile.Serial, Body = mobile.Body, X = mobile.Location.X, Y = mobile.Location.Y, Direction = (int)mobile.Direction };
		}
	}

	public class MobileMovingPacket : Packet
	{
		public int Serial { get; set; }

		public double X { get; set; }

		public double Y { get; set; }

		public int Direction { get; set; }

		public MobileMovingPacket()
			: base(PacketIds.MobileMoving)
		{
		}

		public static MobileMovingPacket Acquire(Mobile mobile)
		{
			return new MobileMovingPacket { Serial = mobile.Serial, X = mobile.Location.X, Y = mobile.Location.Y, Direction = (int)mobile.Direction };
		}
	}

	public class MobileRemovingPacket : Packet
	{
		public int Serial { get; set; }

		public MobileRemovingPacket()
			: base(PacketIds.MobileRemoving)
		{
		}

		public static MobileRemovingPacket Acquire(Mobile mobile)
		{
			return new MobileRemovingPacket { Serial = mobile.Serial };
		}
	}

	public class PlayerDeathPacket : Packet
	{
		public int Serial { get; set; }

		public PlayerDeathPacket()
			: base(PacketIds.PlayerDeath)
		{
		}

		public static PlayerDeathPacket Acquire(PlayerMobile player)
		{
			return new PlayerDeathPacket { Serial = player.Serial };
		}
	}

	public class RespawnRequestPacket : Packet
	{
		public int Serial { get; set; }

		public RespawnRequestPacket()
			: base(PacketIds.PlayerRespawnRequest)
		{
		}

		public static RespawnRequestPacket Acquire(PlayerMobile player)
		{
			return new RespawnRequestPacket { Serial = player.Serial };
		}
	}

	public class FireballRequestPacket : Packet
	{
		public double X { get; set; }
		public double Y { get; set; }

		public FireballRequestPacket()
			: base(PacketIds.FireballRequest)
		{
		}
	}

	public class PlayerStatusPacket : Packet
	{
		public int Serial { get; set; }
		public double Body { get; set; }
		public double Mana { get; set; }
		public double Hits { get; set; }
		public double ManaMax { get; set; }
		public double HitsMax { get; set; }
		public double Speed { get; set; }
		public double RegenHits { get; set; }
		public double RegenMana { get; set; }

		public PlayerStatusPacket()
			: base(PacketIds.PlayerStatus)
		{
		}

		public static PlayerStatusPacket Acquire(PlayerMobile player)
		{
			return new PlayerStatusPacket
			{
				Serial = player.Serial,
				Body = player.Body,
				Hits = player.Hits,
				HitsMax = player.HitsMax,
				ManaMax = player.ManaMax,
				Mana = player.Mana,
				Speed = player.MovementSpeed,
				RegenHits = player.HitsRegenRate,
				RegenMana = player.ManaRegenRate,
			};
		}
	}
	#endregion

	#region WORLD PACKET
	public class ChatMessagePacket : Packet
	{
		public int Type { get; set; }

		public int FromSerial { get; set; }

		public string Message { get; set; }

		public ChatMessagePacket()
			: base(PacketIds.ChatMessage)
		{

		}

		public static ChatMessagePacket Acquire(ChatMessageType type, Mobile from, string message)
		{
			return new ChatMessagePacket
			{
				Type = (int)type,
				FromSerial = from != null ? (int)from.Serial : -1,
				Message = message,
			};
		}
	}

	public class EffectPacket : Packet
	{
		public int Effect { get; set; }
		public double X { get; set; }
		public double Y { get; set; }

		public EffectPacket()
			: base(PacketIds.Effect)
		{
		}

		public static EffectPacket Acquire(int effect, double x, double y)
		{
			return new EffectPacket { Effect = effect, X = x, Y = y };
		}
	}
	#endregion
}
