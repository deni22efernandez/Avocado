using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Models.Dtos.OrderDetails
{
	public class OrderDetailsGet
	{
		public int Id { get; set; }
		[Required]
		public int OrderHeaderId { get; set; }
		//public OrderHeader OrderHeader { get; set; }
		[Required]
		public int ProductId { get; set; }		
		public Product Product { get; set; }
		public int Count { get; set; }
	}
}
