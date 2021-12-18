using Avocado.WEB.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Models.ViewModels
{
	public class SummaryVM
	{
		public CustomerDto Customer { get; set; }
		public IList<ShoppingCart> cartItems { get; set; }

	}
}
