using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ClicWars.Api.Manager
{
	public static class AccountManager
	{
		private static readonly TimeSpan TOKEN_EXPIRATION = TimeSpan.FromMinutes(10);

		private static IDictionary<string, AccountToken> s_tokens = new Dictionary<string, AccountToken>();

		public static AccountToken CreateToken(string username)
		{
			var buffer = new UTF8Encoding().GetBytes(DateTime.Now.Ticks + Guid.NewGuid().ToString());
			var hash = MD5.Create().ComputeHash(buffer);
			var token = new AccountToken
			{
				Value = BitConverter.ToString(hash).Replace("-", String.Empty).ToLower(),
				ExpirationDate = DateTime.Now + TOKEN_EXPIRATION,
			};

			if (s_tokens.ContainsKey(username))
			{
				s_tokens[username] = token;
			}
			else
			{
				s_tokens.Add(username, token);
			}

			return token;
		}

		public static bool IsValid(string username, string token)
		{
			return s_tokens.ContainsKey(username)
				&& s_tokens[username].Value == token
				&& s_tokens[username].ExpirationDate > DateTime.Now;
		}
	}

	public class AccountToken
	{
		public string Value { get; set; }

		public DateTime ExpirationDate { get; set; }
	}
}