using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using WebServer.Services.Accounting;

namespace WebServer.Services
{
	[ServiceContract]
	public interface IServerAdminService
	{
		#region Account
		[OperationContract]
		bool ValidateAccount(string username, string password, out AccountResult account);

		[OperationContract]
		AccountResult CreateAccount(string username, string password);
		
		[OperationContract]
		AccountResult GetAccount(string username);
		#endregion

		#region Character
		[OperationContract]
		CharacterResult CreateCharacter(string username, string name, int body);

		[OperationContract]
		List<CharacterResult> GetCharacters(string username);
		#endregion

		#region Game
		[OperationContract]
		string CreateGameToken(string username, int serial);
		#endregion
	}
}
