using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Models.Dtos.OrderDetails
{
	public class OrderDetailsCreate
	{
		[Required]
		public int OrderHeaderId { get; set; }
		[Required]
		public int ProductId { get; set; }
		public int Count { get; set; }
	}
}
