using ClicWars.Api.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ClicWars.Api.Attributes
{
	public class AuthTokenAttribute : AuthorizeAttribute
	{
		protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
		{
			var token = HttpContext.Current.Request["token"];
			var username = HttpContext.Current.Request["username"];

			if (String.IsNullOrEmpty(token) || String.IsNullOrEmpty(username))
				return false;

			return AccountManager.IsValid(username, token);
		}
	}
}