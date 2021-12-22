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
		public async Task UpdateAsync(OrderHeader orderHeader)//PUT
		{
			var orderHeaderFromDb = await _context.OrderHeaders.FirstOrDefaultAsync(x => x.Id == orderHeader.Id);
			if (orderHeaderFromDb != null) {
				orderHeaderFromDb.OrderDate = orderHeader.OrderDate;
				orderHeaderFromDb.OrderTotal = orderHeader.OrderTotal;
				orderHeaderFromDb.UserId = orderHeader.UserId;
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
		public async Task Update(OrderHeader orderHeader)//PATCH
		{
			var _orderFromDb = await _context.OrderHeaders.FirstOrDefaultAsync(x => x.Id == orderHeader.Id);
			if (_orderFromDb != null)
			{
				_orderFromDb.Carrier = orderHeader.Carrier;
				_orderFromDb.TrackingNumber = orderHeader.TrackingNumber;
			}
			 _context.OrderHeaders.Update(_orderFromDb);			
		}

		
	}
}
