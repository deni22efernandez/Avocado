using Avocado.API.DataAccess;
using Avocado.API.Models;
using Avocado.API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
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
		public async Task UpdateAsync(OrderHeader orderHeader)
		{
			var orderHeaderFromDb = await _context.OrderHeaders.FirstOrDefaultAsync(x => x.Id == orderHeader.Id);
			if (orderHeaderFromDb != null) {
				orderHeaderFromDb.OrderDate = orderHeader.OrderDate;
				orderHeaderFromDb.OrderTotal = orderHeader.OrderTotal;
				orderHeaderFromDb.UserId = orderHeader.UserId;
				orderHeaderFromDb.PaymentTypeId = orderHeader.PaymentTypeId;
				orderHeaderFromDb.OrderStatus = orderHeader.OrderStatus;
				orderHeaderFromDb.PaymentStatus = orderHeader.PaymentStatus;
				orderHeaderFromDb.Carrier = orderHeader.Carrier;
				orderHeaderFromDb.SessionId = orderHeader.SessionId;
				orderHeaderFromDb.PaymentIntentId = orderHeader.PaymentIntentId;
				orderHeaderFromDb.Name = orderHeader.Name;
				orderHeaderFromDb.LastName = orderHeader.LastName;
				orderHeaderFromDb.Email = orderHeader.Email;
				orderHeaderFromDb.PhoneNumber = orderHeader.PhoneNumber;
				orderHeaderFromDb.PostalCode = orderHeader.PostalCode;
				orderHeaderFromDb.StreetAddress = orderHeader.StreetAddress;
				orderHeaderFromDb.City = orderHeader.City;
				orderHeaderFromDb.State = orderHeader.State;
				
			}
		}

		
	}
}
