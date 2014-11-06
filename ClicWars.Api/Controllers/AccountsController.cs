using ClicWars.Api.Manager;
using ClicWars.Api.ServerAdminService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace ClicWars.Api.Controllers
{
	[EnableCors("*", "*", "*")]
	public class AccountsController : ApiController
	{
		private ServerAdminServiceClient m_service;

		public AccountsController()
		{
			m_service = new ServerAdminServiceClient();
		}

		[ResponseType(typeof(AccountConfirmation))]
		public IHttpActionResult Get([FromUri]Account account)
		{
			if (!ModelState.IsValid)
				return BadRequest("Invalid account information.");

			AccountResult result;

			if (!m_service.ValidateAccount(account.Username, account.Password, out result))
			{
				return BadRequest("Validation failed.");
			}

			var token = AccountManager.CreateToken(result.Username);

			var confirmation = new AccountConfirmation
			{
				Username = result.Username,
				Token = token.Value,
				Expires = token.ExpirationDate.Ticks
			};

			return Ok(confirmation);
		}

		[ResponseType(typeof(AccountConfirmation))]
		public IHttpActionResult Post([FromBody]Account account)
		{
			if (!ModelState.IsValid)
				return BadRequest("Invalid account information.");

			var accountResult = m_service.CreateAccount(account.Username, account.Password);

			if (accountResult == null)
			{
				return BadRequest("Can't create the account (invalid username, or already taken).");
			}

			var token = AccountManager.CreateToken(accountResult.Username);

			var confirmation = new AccountConfirmation
			{
				Username = accountResult.Username,
				Token = token.Value,
				Expires = token.ExpirationDate.Ticks
			};

			return Ok(confirmation);
		}
	}

	public class Account
	{
		[Required]
		[StringLength(50, MinimumLength = 2)]
		public string Username { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 2)]
		public string Password { get; set; }
	}

	public class AccountConfirmation
	{
		public string Username { get; set; }

		public string Token { get; set; }

		public long Expires { get; set; }
	}
}
