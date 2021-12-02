
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Repository.IRepository
{
	public interface IUnitOfWork : IDisposable
	{
		IProductRepository ProductRepository { get; }
		IPaymentTypeRepository PaymentTypeRepository { get; }
		IUserRepository UserRepository { get; }
		IOrderDetailRepository OrderDetailRepository { get; }
		IOrderHeaderRepository OrderHeaderRepository { get; }
		ICategoryRepository CategoryRepository { get; }
		void Save();
	}
}
