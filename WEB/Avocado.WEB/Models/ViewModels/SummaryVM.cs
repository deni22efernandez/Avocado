using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Models.ViewModels
{
	public class SummaryVM
	{
		public User Customer { get; set; }
		public IEnumerable<ShoppingCart> cartItems { get; set; }

	}
}
