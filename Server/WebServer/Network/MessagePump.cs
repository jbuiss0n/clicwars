using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fleck;
using WebServer.Network.Packet;
using WebServer.Network.Serialization;

namespace WebServer.Network
{
	public class MessagePump
	{
		private ISerializer m_packetParser;

		private Queue<Client> m_queue;
		private Queue<Client> m_workingQueue;

		public ISerializer Serializer { get { return m_packetParser; } }

		public MessagePump()
		{
			m_packetParser = new JsonSerializer();

			m_queue = new Queue<Client>();
			m_workingQueue = new Queue<Client>();
		}

		public void Slice(IWebSocketConnection[] accepted)
		{
			for (int i = 0; i < accepted.Length; ++i)
			{
				Client client = new Client(accepted[i], this);
				client.Start();
			}

			lock (this)
			{
				Queue<Client> swap = m_workingQueue;
				m_workingQueue = m_queue;
				m_queue = swap;
			}

			while (m_workingQueue.Count > 0)
			{
				Client client = m_workingQueue.Dequeue();

				if (client.Running)
					HandleReceive(client);
			}
		}

		private void HandleReceive(Client client)
		{
			var messageQueue = client.Dequeue();

			foreach (string message in messageQueue)
			{
				var strId = message.Substring(0, 4);
				var strPacket = message.Substring(4);

				var id = Int32.Parse(strId);

				var handler = PacketHandlers.GetHandler(id);
				var packet = (Packet.Packet)m_packetParser.Deserialize(handler.Type, strPacket);

				handler.OnReceive(client, packet);
			}
		}

		public void OnReceive(Client client)
		{
			lock (this)
				m_queue.Enqueue(client);

			Core.Set();
		}
	}
}
