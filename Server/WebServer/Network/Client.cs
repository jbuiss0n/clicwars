using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fleck;
using System.Collections;
using WebServer.Network.Packet;

namespace WebServer.Network
{
	public sealed class Client : IDisposable
	{
		private static List<Client> s_clients = new List<Client>();

		private static string[] m_emptyQueue = new string[0];

		private Guid m_uid;
		private IWebSocketConnection m_socket;
		private MessagePump m_messagePump;

		private Queue<string> m_messageQueue;
		private object m_messageSyncRoot;

		private bool m_running;
		private bool m_disposing;

		public PlayerMobile Player { get; set; }

		public bool Running { get { return m_running; } }
		public IWebSocketConnection Socket { get { return m_socket; } }

		public static List<Client> Clients { get { return s_clients; } }

		public Client(IWebSocketConnection socket, MessagePump messagePump)
		{
			m_uid = Guid.NewGuid();

			m_socket = socket;
			m_messagePump = messagePump;

			m_messageQueue = new Queue<string>();
			m_messageSyncRoot = ((ICollection)m_messageQueue).SyncRoot;

			m_running = false;
			m_disposing = false;

			s_clients.Add(this);
		}

		public void Start()
		{
			m_socket.OnMessage = (message) => OnMessage(message);
			m_socket.OnBinary = (buffer) => OnBinary(buffer);
			m_socket.OnClose = () => OnClose();

			m_running = true;
		}

		private void OnBinary(byte[] buffer)
		{
			throw new NotImplementedException();
		}

		private void OnMessage(string message)
		{
			lock (m_messageSyncRoot)
				m_messageQueue.Enqueue(message);

			m_messagePump.OnReceive(this);
		}

		private void OnClose()
		{
			//FIXME : CALL DISCONNECT ?
			if (Player != null)
				Player.Client = null;

			Dispose();
		}

		public void Send(Packet.Packet packet)
		{
			var message = m_messagePump.Serializer.Serialize(packet);
			m_socket.Send(String.Format("{0:0000}{1}", packet.Id, message));
		}

		public void Dispose()
		{
			if (m_disposing)
				return;

			m_disposing = true;

			m_socket.Close();
			m_socket = null;
			m_running = false;

			s_clients.Remove(this);
			GC.SuppressFinalize(this);
		}

		public string[] Dequeue()
		{
			string[] array;

			lock (m_messageSyncRoot)
			{
				if (m_messageQueue.Count == 0)
					return m_emptyQueue;

				array = m_messageQueue.ToArray();
				m_messageQueue.Clear();
			}
			return array;
		}

		public override string ToString()
		{
			return m_socket != null ? m_socket.ConnectionInfo.ClientIpAddress : String.Empty;
		}
	}
}
