using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Dungeon.Data.Tests
{
	public class Repository<T>
		where T : class
	{
		private DbSet<T> m_objectset;

		public UnitOfWork UnitOfWork { get; set; }

		protected DbSet<T> ObjectSet
		{
			get
			{
				if (m_objectset == null)
				{
					m_objectset = UnitOfWork.Context.Set<T>();
				}
				return m_objectset;
			}
		}

		protected virtual IQueryable<T> Where(Expression<Func<T, bool>> expression)
		{
			return ObjectSet.Where(expression);
		}

		protected virtual T Find(Expression<Func<T, bool>> expression)
		{
			return ObjectSet.FirstOrDefault(expression);
		}

		public virtual void Add(T entity)
		{
			UnitOfWork.Add(entity);
		}

		public virtual void Delete(T entity)
		{
			UnitOfWork.Delete(entity);
		}

		public virtual void Update(T entity)
		{
			UnitOfWork.Update(entity);
		}
	}
}
