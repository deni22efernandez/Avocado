using Avocado.API.DataAccess;
using Avocado.API.Models;
using Avocado.API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Repository
{
	public class PaymentTypeRepository : Repository<PaymentType>, IPaymentTypeRepository
	{
		private readonly ApplicationDbContext _context;
		public PaymentTypeRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}
		public void Update(PaymentType paymentType)
		{
			throw new NotImplementedException();
		}
	}
}
