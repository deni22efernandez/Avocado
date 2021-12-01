using Avocado.API.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Services
{
	public interface IUnitOfWork:IDisposable
	{
		ICategoryRepository CategoryRepository { get; }
		void Save();
	}
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _contxt;
		public UnitOfWork(ApplicationDbContext contxt)
		{
			_contxt = contxt;
			CategoryRepository = new CategoryRepository(_contxt);
		}
		public ICategoryRepository CategoryRepository { get; private set; }
		public void Save()
		{
			_contxt.SaveChanges();
		}

		public void Dispose()
		{
			_contxt.Dispose();
		}
	}
}
