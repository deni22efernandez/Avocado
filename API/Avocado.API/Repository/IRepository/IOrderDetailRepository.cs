using Avocado.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Repository.IRepository
{
	public interface IOrderDetailRepository:IRepository<OrderDetail>
	{
		Task UpdateAsync(OrderDetail orderDetail);
	}
}
