using Avocado.WEB.CustomTagHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Models.ViewModels
{
	public class OrderHeaderIndexVM
	{
		public IEnumerable<OrderHeader> OrderHeaders { get; set; }
		public PaginationModel PaginationModel { get; set; }
	}
}
