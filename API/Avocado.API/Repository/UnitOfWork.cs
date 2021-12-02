using Avocado.API.DataAccess;
using Avocado.API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Repository
{
	
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _contxt;
		public UnitOfWork(ApplicationDbContext contxt)
		{
			_contxt = contxt;
			CategoryRepository = new CategoryRepository(_contxt);
			ProductRepository = new Products(_contxt);
			PaymentTypeRepository = new PaymentTypeRepository(_contxt);
			UserRepository = new UserRepository(_contxt);
			OrderHeaderRepository = new OrderHeaderRepository(_contxt);
			OrderDetailRepository = new OrderDetailRepository(_contxt);
		}
		public ICategoryRepository CategoryRepository { get; private set; }
		public IProductRepository ProductRepository { get; private set; }
		public IPaymentTypeRepository PaymentTypeRepository { get; private set; }
		public IUserRepository UserRepository { get; private set; }
		public IOrderDetailRepository OrderDetailRepository { get; private set; }
		public IOrderHeaderRepository OrderHeaderRepository { get; private set; }

		public async Task SaveAsync()
		{
			await _contxt.SaveChangesAsync();
		}

		public void Dispose()
		{
			_contxt.Dispose();
		}
	}
}
