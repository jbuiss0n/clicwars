using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeon.Data.Repositories;
using Dungeon.Data.Entities;

namespace Dungeon.Data.Tests.Repositories
{
	public class AccountRepository : Repository<Account>, IAccountRepository 
	{
		public new IUnitOfWork UnitOfWork
		{
			get
			{
				return UnitOfWork;
			}
			set
			{
				UnitOfWork = value;
			}
		}

		public AccountRepository(UnitOfWork unitOfWork)
		{
			UnitOfWork = unitOfWork;
		}

		public Account GetByUsername(string username)
		{
			return ObjectSet.FirstOrDefault(a => a.Username == username);
		}

		public Account GetById(int id_Account)
		{
			return ObjectSet.FirstOrDefault(a => a.id_Account == id_Account);
		}
	}
}
