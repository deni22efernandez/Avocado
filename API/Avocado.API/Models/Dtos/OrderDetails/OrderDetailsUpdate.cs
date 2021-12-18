using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Models.Dtos.OrderDetails
{
	public class OrderDetailsUpdate
	{
		public int Id { get; set; }	
		public int OrderHeaderId { get; set; }		
		public int ProductId { get; set; }
		public int Count { get; set; }
	}
}
