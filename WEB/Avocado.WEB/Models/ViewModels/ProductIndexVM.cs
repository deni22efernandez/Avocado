using Avocado.WEB.CustomTagHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Models.ViewModels
{
	public class ProductIndexVM
	{
		public List<Product> Products { get; set; }
		public PaginationModel PaginationModel { get; set; }
	}
}
