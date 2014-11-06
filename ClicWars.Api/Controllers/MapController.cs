using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace ClicWars.Api.Controllers
{
	[EnableCors("*", "*", "*")]
	public class MapController : ApiController
	{
		private static string MAP_DIRECTORY = ConfigurationManager.AppSettings["PATH_MAPS"];

		private static string[] Maps = new string[] { "main" };

		[ResponseType(typeof(Map))]
		public IHttpActionResult Get(int id)
		{
			var name = Maps[id];
			var mapPath = Path.Combine(MAP_DIRECTORY, String.Format("{0}-{1}", id, name));

			if (!Directory.Exists(mapPath))
				return NotFound();

			var index = 0;
			var map = new Map();
			var layerPath = Path.Combine(mapPath, String.Format("{0}.layer", index++));

			while (System.IO.File.Exists(layerPath))
			{
				map.Layers.Add(LoadLayer(layerPath));
				layerPath = Path.Combine(mapPath, String.Format("{0}.layer", index++));
			}

			return Ok(map);
		}

		private List<List<int>> LoadLayer(string layerPath)
		{
			var layer = new List<List<int>>();
			var rows = System.IO.File.ReadAllLines(layerPath);

			foreach (var row in rows)
			{
				layer.Add(new List<int>());
				foreach (var column in row.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
				{
					layer.Last().Add(column != "-" ? Int32.Parse(column) : -1);
				}
			}

			return layer;
		}
	}

	public class Map
	{
		public List<List<List<int>>> Layers { get; private set; }

		public Map()
		{
			Layers = new List<List<List<int>>>();
		}
	}
}