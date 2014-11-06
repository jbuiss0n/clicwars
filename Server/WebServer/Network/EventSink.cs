using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using Fleck;
using WebServer.Accounting;

namespace WebServer.Network
{
	#region Delegates
	public delegate void SocketConnectEventHandler(SocketConnectEventArgs e);

	public delegate void ConnectedEventHandler(ConnectedEventArgs e);
	public delegate void DisconnectedEventHandler(DisconnectedEventArgs e);

	public delegate void CrashedEventHandler(CrashedEventArgs e);
	public delegate void ShutdownEventHandler();

	public delegate void WorldLoadEventHandler();
	public delegate void WorldSaveEventHandler();

	public delegate void MovementRequestEventHandler(MovementRequestEventArgs e);

	public delegate void PlayerDeathEventHandler(PlayerDeathEventArgs e);
	#endregion

	#region EventArgs
	public class SocketConnectEventArgs : EventArgs
	{
		private IWebSocketConnection m_Socket;
		private bool m_AllowConnection;

		public IWebSocketConnection Socket { get { return m_Socket; } }
		public bool AllowConnection { get { return m_AllowConnection; } set { m_AllowConnection = value; } }

		public SocketConnectEventArgs(IWebSocketConnection s)
		{
			m_Socket = s;
			m_AllowConnection = true;
		}
	}

	public class ConnectedEventArgs : EventArgs
	{
		public PlayerMobile Player { get; private set; }

		public ConnectedEventArgs(PlayerMobile mobile)
		{
			Player = mobile;
		}
	}

	public class DisconnectedEventArgs : EventArgs
	{
		public PlayerMobile Player { get; private set; }

		public DisconnectedEventArgs(PlayerMobile mobile)
		{
			Player = mobile;
		}
	}

	public class CrashedEventArgs : EventArgs
	{
		private Exception m_Exception;
		private bool m_Close;

		public Exception Exception { get { return m_Exception; } }
		public bool Close { get { return m_Close; } set { m_Close = value; } }

		public CrashedEventArgs(Exception e)
		{
			m_Exception = e;
		}
	}

	public class MovementRequestEventArgs : EventArgs
	{
		private Client m_client;
		private int m_x;
		private int m_y;
		private int m_body;

		public Client Client { get { return m_client; } }
		public int X { get { return m_x; } }
		public int Y { get { return m_y; } }
		public int Body { get { return m_body; } }

		public MovementRequestEventArgs(Client client, int x, int y, int body)
		{
			m_client = client;
			m_x = x;
			m_y = y;
			m_body = body;
		}
	}

	public class PlayerDeathEventArgs : EventArgs
	{
		public PlayerMobile Player { get; private set; }

		public Mobile Killer { get; private set; }

		public PlayerDeathEventArgs(PlayerMobile player, Mobile killer)
		{
			Player = player;
			Killer = killer;
		}
	}
	#endregion

	public class EventSink
	{
		#region Events
		public static event SocketConnectEventHandler SocketConnect;

		public static event ConnectedEventHandler Connected;
		public static event DisconnectedEventHandler Disconnected;

		public static event CrashedEventHandler Crashed;
		public static event ShutdownEventHandler Shutdown;

		public static event WorldLoadEventHandler WorldLoad;
		public static event WorldSaveEventHandler WorldSave;

		public static event MovementRequestEventHandler MovementRequest;

		public static event PlayerDeathEventHandler PlayerDeath;
		#endregion

		public static void InvokeSocketConnect(SocketConnectEventArgs e)
		{
			if (SocketConnect != null)
				SocketConnect(e);
		}

		public static void InvokeConnected(ConnectedEventArgs e)
		{
			if (Connected != null)
				Connected(e);
		}

		public static void InvokeDisconnected(DisconnectedEventArgs e)
		{
			if (Disconnected != null)
				Disconnected(e);
		}

		public static void InvokeCrashed(CrashedEventArgs e)
		{
			if (Crashed != null)
				Crashed(e);
		}

		public static void InvokeShutdown()
		{
			if (Shutdown != null)
				Shutdown();
		}

		public static void InvokeWorldLoad()
		{
			if (WorldLoad != null)
				WorldLoad();
		}

		public static void InvokeWorldSave()
		{
			if (WorldSave != null)
				WorldSave();
		}

		public static void InvokeMovementRequest(MovementRequestEventArgs e)
		{
			if (MovementRequest != null)
				MovementRequest(e);
		}

		public static void InvokePlayerDeath(PlayerDeathEventArgs e)
		{
			if (PlayerDeath != null)
				PlayerDeath(e);
		}
	}
}