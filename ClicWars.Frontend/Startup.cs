﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="">
//   Copyright © 2014 
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: Microsoft.Owin.OwinStartup(typeof(App.ClicWars.Frontend.Startup))]

namespace App.ClicWars.Frontend
{
	using Owin;

	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			//// For more information on how to configure your application, visit:
			//// http://go.microsoft.com/fwlink/?LinkID=316888

			this.ConfigureAuth(app);
		}
	}
}
