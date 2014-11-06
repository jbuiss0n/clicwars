using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Dungeon.Data.Tests
{
	public partial class DungeonContext : DbContext
	{
		public const string ConnectionString = "name=DungeonEntities";
		public const string ContainerName = "DungeonEntities";

		public DungeonContext()
			: base(ConnectionString)
		{
			this.Configuration.LazyLoadingEnabled = true;
			this.Configuration.ValidateOnSaveEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			throw new UnintentionalCodeFirstException();
		}
	}
}
