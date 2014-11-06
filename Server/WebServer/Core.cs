using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServer.Network;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Configuration;

namespace WebServer
{
	public static class Core
	{
		private static Thread s_timer;
		private static Server s_server;
		private static AutoResetEvent s_signal;

		public static Thread Thread { get; private set; }
		public static Process Process { get; private set; }
		public static Assembly Assembly { get; private set; }

		public static bool Closing { get; private set; }

		public static void Setup()
		{
			Thread = Thread.CurrentThread;
			Process = Process.GetCurrentProcess();
			Assembly = Assembly.GetExecutingAssembly();

			var invokeConfig = new List<MethodInfo>();
			var invokeInit = new List<MethodInfo>();
			var types = Assembly.GetExecutingAssembly().GetTypes();

			foreach (var type in types)
			{
				var methodConfig = type.GetMethod("Configure", BindingFlags.Static | BindingFlags.Public);
				var methodInit = type.GetMethod("Initialize", BindingFlags.Static | BindingFlags.Public);

				if (methodConfig != null)
					invokeConfig.Add(methodConfig);

				if (methodInit != null)
					invokeInit.Add(methodInit);
			}

			invokeConfig.Sort(new CallPriorityComparer());
			invokeInit.Sort(new CallPriorityComparer());

			foreach (var invoke in invokeConfig)
				invoke.Invoke(null, null);

			World.Load();

			foreach (var invoke in invokeInit)
				invoke.Invoke(null, null);

		}

		public static void Start()
		{
			s_signal = new AutoResetEvent(true);
			s_server = new Server(ConfigurationManager.AppSettings["ServerBinding"]);

			var timerThread = new Timer.TimerThread();
			s_timer = new Thread(new ThreadStart(timerThread.TimerMain));
			s_timer.Name = "Timer Thread";

			s_server.Start();
			s_timer.Start();

			while (!Closing && s_signal.WaitOne())
			{
				Timer.Slice();
				s_server.Slice();
			}
		}

		public static void Set()
		{
			s_signal.Set();
		}

		public static void Stop()
		{
			Closing = true;
			s_signal.Set();
		}
	}
}
