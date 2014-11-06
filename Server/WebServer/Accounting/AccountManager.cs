using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using WebServer.Network;
using WebServer.Persistence;

namespace WebServer.Accounting
{
	public class AccountManager
	{
		public static void Configure()
		{
			EventSink.WorldLoad += new WorldLoadEventHandler(Load);
			EventSink.WorldSave += new WorldSaveEventHandler(Save);
		}

		private static Dictionary<string, Account> s_accounts = new Dictionary<string, Account>();

		private static TimeSpan s_deleteDelay = TimeSpan.FromDays(7.0);

		public static int Count
		{
			get { return s_accounts.Count; }
		}

		public static ICollection<Account> GetAccounts()
		{
			return s_accounts.Values;
		}

		public static Account GetAccount(string username)
		{
			if (String.IsNullOrEmpty(username))
				return null;

			Account a;

			s_accounts.TryGetValue(username, out a);

			return a;
		}

		public static void Add(Account account)
		{
			s_accounts[account.Username] = account;
		}

		public static void Remove(string username)
		{
			s_accounts.Remove(username);
		}

		public static void Load()
		{
			s_accounts = new Dictionary<string, Account>(32, StringComparer.OrdinalIgnoreCase);

			var path = PathManager.GetFullPath(System.IO.Path.Combine(PathManager.PATH_SAVES, PathManager.PATH_ACCOUNTS));

			if (!File.Exists(path))
				return;

			XmlDocument xml = new XmlDocument();
			xml.Load(path);

			foreach (XmlElement xmlAccount in xml.SelectNodes("accounts/account"))
			{
				Account account = new Account();
				account.Load(xmlAccount);
				Add(account);
			}
		}

		public static void Save()
		{
			var path = PathManager.GetFullPath(System.IO.Path.Combine(PathManager.PATH_SAVES, PathManager.PATH_ACCOUNTS));

			PathManager.EnsureDirectory(path);

			using (StreamWriter op = new StreamWriter(path))
			{
				XmlTextWriter xml = new XmlTextWriter(op);

				xml.Formatting = Formatting.Indented;
				xml.IndentChar = '\t';
				xml.Indentation = 1;

				xml.WriteStartDocument(true);
				xml.WriteStartElement("accounts");
				xml.WriteAttributeString("count", s_accounts.Count.ToString());

				foreach (Account a in GetAccounts())
					a.Save(xml);

				xml.WriteEndElement();
			}
		}

		//TODO : Add some code errors (out param)
		public static Account CreateAccount(string username, string password)
		{
			username = username.Trim();
			password = password.Trim();

			if (AccountManager.GetAccount(username) != null)
				return null;

			if (username.Length == 0 || password.Length == 0)
				return null;

			bool isSafe = true;

			for (int i = 0; isSafe && i < username.Length; ++i)
				isSafe = (username[i] >= 0x20 && username[i] < 0x80);

			for (int i = 0; isSafe && i < password.Length; ++i)
				isSafe = (password[i] >= 0x20 && password[i] < 0x80);

			if (!isSafe)
				return null;

			var account = new Account(username, password);
			Add(account);
			return account;
		}

		public static bool IsValid(string username, string password, out Account account)
		{
			account = AccountManager.GetAccount(username);

			if (account == null || !account.CheckPassword(password))
			{
				account = null;
				return false;
			}

			return true;
		}
	}
}