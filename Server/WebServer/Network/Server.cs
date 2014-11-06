using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fleck;
using System.Threading;
using System.Collections;
using System.Reflection;

namespace WebServer.Network
{
	public sealed class Server : IDisposable
	{
		private static IWebSocketConnection[] m_emptySockets = new IWebSocketConnection[0];

		private WebSocketServer m_server;
		private MessagePump m_messagePump;

		private Queue<IWebSocketConnection> m_accepted;
		private object m_acceptedSyncRoot;

		public Server(string location)
		{
			m_accepted = new Queue<IWebSocketConnection>();
			m_acceptedSyncRoot = ((ICollection)m_accepted).SyncRoot;

			m_server = new WebSocketServer(location);
			m_messagePump = new MessagePump();
		}

		public void Start()
		{
			m_server.Start(socket =>
			{
				socket.OnOpen = () => OnOpen(socket);
			});
		}

		public void Slice()
		{
			IWebSocketConnection[] array;

			lock (m_acceptedSyncRoot)
			{
				if (m_accepted.Count == 0)
					array = m_emptySockets;
				else
					array = m_accepted.ToArray();
				m_accepted.Clear();
			}

			m_messagePump.Slice(array);
		}

		private void OnOpen(IWebSocketConnection socket)
		{
			if (VerifySocket(socket))
				Enqueue(socket);
			else
				Release(socket);
		}

		private void Release(IWebSocketConnection socket)
		{
			socket.Close();
			socket = null;
		}

		private void Enqueue(IWebSocketConnection socket)
		{
			lock (m_acceptedSyncRoot)
			{
				m_accepted.Enqueue(socket);
			}

			Core.Set();
		}

		private bool VerifySocket(IWebSocketConnection socket)
		{
			var args = new SocketConnectEventArgs(socket);
			EventSink.InvokeSocketConnect(args);
			return args.AllowConnection;
		}

		public void Dispose()
		{
			m_server.Dispose();
		}
	}
}
