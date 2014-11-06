// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="">
//   Copyright © 2014 
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace App.ClicWars.Frontend
{
	using System.Web;
	using System.Web.Optimization;

	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/content/css/app")
				.Include("~/content/app.css"));

			bundles.Add(new ScriptBundle("~/js/jquery")
				.Include("~/scripts/vendor/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/js/app")
				.Include(
					"~/scripts/vendor/angular-route.js",
					"~/scripts/vendor/angular-resource.js",

					"~/scripts/config/config.local.js",
					"~/scripts/utils.js",
					"~/scripts/gametime.js",
					"~/scripts/app.js")

				.Include("~/scripts/packets/Packet.js")

				.IncludeDirectory("~/scripts/sprites/", "*.js", true)
				.IncludeDirectory("~/scripts/packets/", "*.js", true)
				.IncludeDirectory("~/scripts/services/", "*.js", true)
				.IncludeDirectory("~/scripts/controllers/", "*.js", true)
				.IncludeDirectory("~/scripts/directives/", "*.js", true));
		}
	}
}
