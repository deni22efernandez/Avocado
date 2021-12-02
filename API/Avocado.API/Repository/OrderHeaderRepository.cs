using Avocado.API.DataAccess;
using Avocado.API.Models;
using Avocado.API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Repository
{
	public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
	{
		private readonly ApplicationDbContext _context;
		public OrderHeaderRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}
		public void Update(OrderHeader orderHeader)
		{
			throw new NotImplementedException();
		}
	}
}
