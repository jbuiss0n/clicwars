// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="">
//   Copyright © 2014 
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace App.ClicWars.Frontend
{
	using System.Web.Routing;

	using App.ClicWars.Frontend.Routing;

	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.Add("Default", new DefaultRoute());
		}
	}
}
