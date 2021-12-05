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
	public class Products : Repository<Product> , IProductRepository
	{
		private readonly ApplicationDbContext _context;
		public Products(ApplicationDbContext context):base(context)
		{
			_context = context;
		}

		public async Task UpdateAsync(Product product)
		{
			var objFromDb = await _context.Products.FirstOrDefaultAsync(x => x.Id == product.Id);
			if (product.ImgUri != null)
			{
				objFromDb.ImgUri = product.ImgUri;
			}
			objFromDb.Name = product.Name;
			objFromDb.Description = product.Description;
			objFromDb.CategoryId = product.CategoryId;
			objFromDb.Price = product.Price;
			
			 _context.Products.Update(objFromDb);
		}
	}
}
