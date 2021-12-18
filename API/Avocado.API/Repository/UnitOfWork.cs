using Avocado.API.DataAccess;
using Avocado.API.Models;
using Avocado.API.Repository.IRepository;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Repository
{
	
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _contxt;
		public UnitOfWork(ApplicationDbContext contxt, IOptions<AppSettings> options)
		{
			_contxt = contxt;
			CategoryRepository = new CategoryRepository(_contxt);
			ProductRepository = new Products(_contxt);
			UserRepository = new UserRepository(_contxt, options);
			OrderHeaderRepository = new OrderHeaderRepository(_contxt);
			OrderDetailRepository = new OrderDetailRepository(_contxt);
			Stored_Proc_Calls = new Stored_Proc_Calls(_contxt);
		}
		public ICategoryRepository CategoryRepository { get; private set; }
		public IProductRepository ProductRepository { get; private set; }
		public IUserRepository UserRepository { get; private set; }
		public IOrderDetailRepository OrderDetailRepository { get; private set; }
		public IOrderHeaderRepository OrderHeaderRepository { get; private set; }

		public IStored_Proc_Calls Stored_Proc_Calls { get; private set; }

		public async Task<bool> SaveAsync()
		{
			var result = await _contxt.SaveChangesAsync() > 0 ? true : false;
			return result;
		}

		public void Dispose()
		{
			_contxt.Dispose();
		}
	}
}
