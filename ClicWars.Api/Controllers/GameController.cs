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
	public class GameController : ApiController
	{
		private ServerAdminServiceClient m_service;

		public GameController()
		{
			m_service = new ServerAdminServiceClient();
		}

		[AuthToken]
		public IHttpActionResult Get(string username, int serial)
		{
			var token = m_service.CreateGameToken(username, serial);

			return Ok(new { Value = token });
		}
	}
}
