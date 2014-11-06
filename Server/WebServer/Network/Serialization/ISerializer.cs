using System;
namespace WebServer.Network.Serialization
{
	public interface ISerializer
	{
		object Deserialize(Type type, string strPacket);
		string Serialize(object packet);
	}
}
