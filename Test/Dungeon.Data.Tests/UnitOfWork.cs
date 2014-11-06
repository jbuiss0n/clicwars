using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data;

namespace Dungeon.Data.Tests
{
	public class UnitOfWork : IUnitOfWork
	{
		public DbContext Context { get; set; }

		public UnitOfWork()
		{
			Context = new DungeonContext();
		}

		public void Commit()
		{
			Context.SaveChanges();
		}

		public void Dispose()
		{
			Context.Dispose();
		}

		public void Add<T>(T entity)
			where T : class
		{
			Context.Set<T>().Add(entity);
		}

		public void Delete<T>(T entity)
			where T : class
		{
			if (Context.Entry(entity).State == EntityState.Detached)
			{
				Context.Set<T>().Attach(entity);
			}
			Context.Set<T>().Remove(entity);
		}

		public void Update<T>(T entity)
			where T : class
		{
			Context.Set<T>().Attach(entity);
			Context.Entry(entity).State = EntityState.Modified;
		}
	}
}
