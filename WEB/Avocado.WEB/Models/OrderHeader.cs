using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Models
{
	
	public class OrderHeader
	{
		public int Id { get; set; }
		[Required]
		public DateTime OrderDate { get; set; }
		[Required]
		public double OrderTotal { get; set; }				
		public int UserId { get; set; }	
		public string OrderStatus { get; set; }
		public string PaymentStatus { get; set; }
		public string TrackingNumber { get; set; }
		public string Carrier { get; set; }
		public string SessionId { get; set; }
		public string PaymentIntentId { get; set; }
		public string Name { get; set; }
		public string LastName { get; set; }
		public string FullName { get { return Name + ", "+ LastName; } }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public int PostalCode { get; set; }
		public string StreetAddress { get; set; }
		public string City { get; set; }
		public string State { get; set; }

	}
}
