using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Models.ViewModels
{
	public class ShoppingCartVM
	{
		public Product Product { get; set; }
		public bool ExistsInCart { get; set; } = false;
		public int Count { get; set; }

	}
}
