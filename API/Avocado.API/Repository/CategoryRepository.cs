using Avocado.API.DataAccess;
using Avocado.API.Models;
using Avocado.API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Services
{
	
	public class CategoryRepository :  Repository<Category>, ICategoryRepository
	{
		private readonly ApplicationDbContext _context;
		public CategoryRepository(ApplicationDbContext contxt):base(contxt)
		{
			_context = contxt;
		}

		public void Update(Category category)
		{
			_context.Update(category);//
		}
	}
}
