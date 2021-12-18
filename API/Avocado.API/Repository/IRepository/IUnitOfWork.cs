
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Repository.IRepository
{
	public interface IUnitOfWork : IDisposable
	{
		IProductRepository ProductRepository { get; }
		IUserRepository UserRepository { get; }
		IOrderDetailRepository OrderDetailRepository { get; }
		IOrderHeaderRepository OrderHeaderRepository { get; }
		ICategoryRepository CategoryRepository { get; }
		IStored_Proc_Calls Stored_Proc_Calls { get; }
		Task<bool> SaveAsync();
	}
}
