using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Models
{
	public class ShoppingCart
	{
		public Product Product { get; set; }
		public int ProductId { get; set; }
		public int Count { get; set; }
	}
}
