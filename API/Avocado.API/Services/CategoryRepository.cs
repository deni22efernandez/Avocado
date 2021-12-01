using Avocado.API.DataAccess;
using Avocado.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Services
{
	public interface ICategoryRepository : IQuerieService<Category>
	{
		void Update(Category category);
	}
	public class CategoryRepository :  QuerieService<Category>, ICategoryRepository
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
