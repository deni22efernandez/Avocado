﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Models
{
	public class PaymentType
	{
		public int Id { get; set; }
		public string Description { get; set; }
	}
	public class OrderHeader
	{
		public int Id { get; set; }
		[Required]
		public DateTime OrderDate { get; set; }
		[Required]
		public double OrderTotal { get; set; }				
		public User User { get; set; }	
		public PaymentType PaymentType { get; set; }
		public string OrderStatus { get; set; }
		public string PaymentStatus { get; set; }
		public int? TransactionId { get; set; }
		public string Name { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public int PostalCode { get; set; }
		public string StreetAddress { get; set; }
		public string City { get; set; }
		public string State { get; set; }
	}
}