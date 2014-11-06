using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using WebServer.Accounting;
using WebServer.Services.Accounting;

namespace WebServer.Services
{
	public class ServerAdminService : IServerAdminService
	{
		#region Account
		public AccountResult CreateAccount(string username, string password)
		{
			return Mapping(AccountManager.CreateAccount(username, password));
		}

		public bool ValidateAccount(string username, string password, out AccountResult result)
		{
			result = null;
			Account account;

			if (AccountManager.IsValid(username, password, out account))
			{
				result = Mapping(account);
				return true;
			}

			return false;
		}

		public AccountResult GetAccount(string username)
		{
			return Mapping(AccountManager.GetAccount(username));
		}
		#endregion

		#region Character
		public CharacterResult CreateCharacter(string username, string name, int body)
		{
			var account = AccountManager.GetAccount(username);

			if (account.Count == account.Length)
				return null;

			var character = PlayerMobile.Create(name, body);
			account.FreeSlot = character;
			return Mapping(character);
		}

		public List<CharacterResult> GetCharacters(string username)
		{
			var characters = new List<CharacterResult>();
			var account = AccountManager.GetAccount(username);
			for (int i = 0; i < account.Length; i++)
			{
				if (account[i] != null)
					characters.Add(Mapping(account[i]));
			}
			return characters;
		}
		#endregion

		#region Game
		public string CreateGameToken(string username, int serial)
		{
			var account = AccountManager.GetAccount(username);
		
			if (account == null)
				return null;

			var token = Guid.NewGuid().ToString();//TODO : create a stronger token using hash

			account.GameToken = new GameToken(token, serial, DateTime.Now);

			return token;
		}
		#endregion

		private CharacterResult Mapping(PlayerMobile character)
		{
			if (character == null)
				return null;

			return new CharacterResult
			{
				Serial = character.Serial,
				Name = character.Name,
				Body = character.Body,
				CreationDate = character.CreationDate,
				Deaths = character.Deaths,
				Kills = character.Kills,
			};
		}

		private AccountResult Mapping(Account account)
		{
			if (account == null)
				return null;

			return new AccountResult
			{
				Username = account.Username,
			};
		}
	}
}