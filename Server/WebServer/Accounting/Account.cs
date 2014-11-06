using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using WebServer.Network;

namespace WebServer.Accounting
{
	public class Account : IComparable, IComparable<Account>
	{
		public static void Configure()
		{
			EventSink.Connected += new ConnectedEventHandler(EventSink_Connected);
			EventSink.Disconnected += new DisconnectedEventHandler(EventSink_Disconnected);
		}

		public static readonly TimeSpan InactiveDuration = TimeSpan.FromDays(180.0);

		private static SHA1CryptoServiceProvider m_SHA1HashProvider;
		private static byte[] m_HashBuffer;

		private TimeSpan m_totalGameTime;
		private PlayerMobile[] m_characters;
		private string[] m_loginIPs;

		public string[] LoginIPs
		{
			get { return m_loginIPs; }
			set { m_loginIPs = value; }
		}

		public string Username { get; private set; }

		public string Password { get; private set; }

		public DateTime Created { get; private set; }

		public DateTime LastLogin { get; private set; }

		public GameToken GameToken { get; set; }

		public bool Inactive
		{
			get { return ((LastLogin + InactiveDuration) <= DateTime.Now); }
		}

		public TimeSpan TotalGameTime
		{
			get
			{
				for (int i = 0; i < m_characters.Length; i++)
				{
					PlayerMobile m = m_characters[i] as PlayerMobile;

					if (m != null && m.Client != null)
						return m_totalGameTime + (DateTime.Now - m.SessionStart);
				}

				return m_totalGameTime;
			}
			private set
			{
				m_totalGameTime = value;
			}
		}

		public int Count
		{
			get
			{
				int count = 0;

				for (int i = 0; i < this.Length; ++i)
				{
					if (this[i] != null)
						++count;
				}

				return count;
			}
		}

		public int Length
		{
			get { return m_characters.Length; }
		}

		public PlayerMobile FreeSlot
		{
			set
			{
				var index = 0;
				while (index < this.Length && this[index] != null)
				{
					index++;
				}

				if (index >= this.Length)
					return;

				this[index] = value;
			}
		}

		public PlayerMobile this[int index]
		{
			get
			{
				if (index >= 0 && index < m_characters.Length)
				{
					return m_characters[index];
				}

				return null;
			}
			set
			{
				if (index >= 0 && index < m_characters.Length)
				{
					if (m_characters[index] != null)
						m_characters[index].Account = null;

					m_characters[index] = value;

					if (m_characters[index] != null)
						m_characters[index].Account = this;
				}
			}
		}

		public Account()
		{
			m_characters = new PlayerMobile[6];
			m_loginIPs = new string[0];
		}

		public Account(string username, string password)
			: this()
		{
			Username = username;
			SetPassword(password);

			Created = LastLogin = DateTime.Now;
			m_totalGameTime = TimeSpan.Zero;
		}

		public void SetPassword(string plainPassword)
		{
			Password = HashSHA1(Username + plainPassword);
		}

		public bool CheckPassword(string plainPassword)
		{
			return (Password == HashSHA1(Username + plainPassword));
		}

		public override string ToString()
		{
			return Username;
		}

		public int CompareTo(Account other)
		{
			if (other == null)
				return -1;

			return Username.CompareTo(other.Username);
		}

		public int CompareTo(object obj)
		{
			if (obj is Account)
				return this.CompareTo((Account)obj);

			throw new ArgumentException();
		}

		public void Save(XmlTextWriter xml)
		{
			xml.WriteStartElement("account");

			xml.WriteElementString("username", Username);
			xml.WriteElementString("password", Password);

			xml.WriteElementString("created", XmlConvert.ToString(Created, XmlDateTimeSerializationMode.Local));

			xml.WriteElementString("lastLogin", XmlConvert.ToString(LastLogin, XmlDateTimeSerializationMode.Local));

			xml.WriteElementString("totalGameTime", XmlConvert.ToString(TotalGameTime));

			xml.WriteStartElement("characters");
			for (int i = 0; i < m_characters.Length; ++i)
			{
				var character = m_characters[i];

				if (character != null)
				{
					xml.WriteStartElement("character");
					xml.WriteAttributeString("index", i.ToString());
					xml.WriteString(character.Serial.Value.ToString());
					xml.WriteEndElement();
				}
			}
			xml.WriteEndElement();

			if (m_loginIPs.Length > 0)
			{
				xml.WriteStartElement("addressList");

				xml.WriteAttributeString("count", m_loginIPs.Length.ToString());

				for (int i = 0; i < m_loginIPs.Length; ++i)
				{
					xml.WriteStartElement("ip");
					xml.WriteString(m_loginIPs[i].ToString());
					xml.WriteEndElement();
				}

				xml.WriteEndElement();
			}

			xml.WriteEndElement();
		}

		public void Load(XmlElement xml)
		{
			Username = xml["username"].InnerText;
			Password = xml["password"].InnerText;

			Created = XmlConvert.ToDateTime(xml["created"].InnerText, XmlDateTimeSerializationMode.Local);
			LastLogin = XmlConvert.ToDateTime(xml["lastLogin"].InnerText, XmlDateTimeSerializationMode.Local);
			TotalGameTime = XmlConvert.ToTimeSpan(xml["totalGameTime"].InnerText);

			foreach (XmlNode character in xml.SelectNodes("characters/character"))
			{
				int index = XmlConvert.ToInt32(character.Attributes["index"].Value);
				int serial = XmlConvert.ToInt32(character.InnerText);

				if (index >= 0 && index < m_characters.Length)
				{
					m_characters[index] = (PlayerMobile)World.FindMobile(serial);
					m_characters[index].Account = this;
				}
			}
		}

		public static string HashSHA1(string phrase)
		{
			if (m_SHA1HashProvider == null)
				m_SHA1HashProvider = new SHA1CryptoServiceProvider();

			if (m_HashBuffer == null)
				m_HashBuffer = new byte[256];

			int length = Encoding.ASCII.GetBytes(phrase, 0, phrase.Length > 256 ? 256 : phrase.Length, m_HashBuffer, 0);
			byte[] hashed = m_SHA1HashProvider.ComputeHash(m_HashBuffer, 0, length);

			return BitConverter.ToString(hashed);
		}

		private static void EventSink_Connected(ConnectedEventArgs e)
		{
			Account account = e.Player.Account;

			if (account == null)
				return;

			PlayerMobile player = e.Player;

			if (player == null)
				return;

			player.SessionStart = DateTime.Now;
		}

		private static void EventSink_Disconnected(DisconnectedEventArgs e)
		{
			Account account = e.Player.Account;

			if (account == null)
				return;

			PlayerMobile player = e.Player;

			if (player == null)
				return;

			account.m_totalGameTime += DateTime.Now - player.SessionStart;
		}
	}

	public class GameToken
	{
		public static readonly TimeSpan EXPIRATION_TIME = TimeSpan.FromMinutes(3);

		public int Serial { get; private set; }

		public string Value { get; private set; }

		public DateTime CreatedDate { get; private set; }

		public GameToken(string token, int serial, DateTime dateTime)
		{
			Value = token;
			Serial = serial;
			CreatedDate = dateTime;
		}
	}
}