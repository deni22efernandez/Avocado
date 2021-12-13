using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Models
{
	public class OrderDetail
	{
		public int Id { get; set; }
		public OrderHeader OrderHeader { get; set; }
		public Product Product { get; set; }
		public int Count { get; set; }
	}
}
