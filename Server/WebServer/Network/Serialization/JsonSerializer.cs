using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

namespace WebServer.Network.Serialization
{
	public class JsonSerializer : ISerializer
	{
		public object Deserialize(Type type, string strPacket)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();

			return jss.Deserialize(strPacket, type);
		}

		public string Serialize(object packet)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();

			return jss.Serialize(packet);
		}
	}
}