using Avocado.WEB.CustomTagHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Models.ViewModels
{
	public class HomeIndexVM
	{
		public IEnumerable<Category> Categories { get; set; }
		public IEnumerable<Product> Products { get; set; }
		public PaginationModel Pagination { get; set; }
	}
}
