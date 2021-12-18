using Avocado.API.DataAccess;
using Avocado.API.Models;
using Avocado.API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Repository
{
	public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
	{
		private readonly ApplicationDbContext _context;
		public OrderDetailRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}
		public async Task UpdateAsync(OrderDetail orderDetail)
		{
			var detailFromDb = await _context.OrderDetails.FirstOrDefaultAsync(x => x.Id == orderDetail.Id);
			if (detailFromDb != null)
			{
				detailFromDb.OrderHeaderId = orderDetail.OrderHeaderId;
				detailFromDb.ProductId = orderDetail.ProductId;
				detailFromDb.Count = orderDetail.Count;
			}

		}
	}
}
