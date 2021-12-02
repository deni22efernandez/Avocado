using Avocado.API.DataAccess;
using Avocado.API.Models;
using Avocado.API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Repository
{
	public class ProductRepository : Repository<Product> , IProductRepository
	{
		private readonly ApplicationDbContext _context;
		public ProductRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}

		public void Update(Product product)
		{
			throw new NotImplementedException();
		}
	}
}
