using ClicWars.Api.Attributes;
using ClicWars.Api.Manager;
using ClicWars.Api.ServerAdminService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace ClicWars.Api.Controllers
{
	[EnableCors("*", "*", "*")]
	public class CharactersController : ApiController
	{
		private ServerAdminServiceClient m_service;

		public CharactersController()
		{
			m_service = new ServerAdminServiceClient();
		}

		[AuthToken]
		public IHttpActionResult Get(string username)
		{
			var characters = m_service.GetCharacters(username);
			return Ok(characters);
		}

		[AuthToken]
		public IHttpActionResult Post(string username, [FromBody]Character character)
		{
			var result = m_service.CreateCharacter(username, character.Name, character.Body);
			return Ok(result);
		}
	}

	public class Character
	{
		public string Name { get; set; }

		public int Body { get; set; }
	}
}
