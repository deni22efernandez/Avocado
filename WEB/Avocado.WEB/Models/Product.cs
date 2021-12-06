﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Models
{
	public class Product
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[MaxLength(int.MaxValue)]
		public string Description { get; set; }
		public double Price { get; set; }
		[Required]
		public int CategoryId { get; set; }
		
		public Category Category { get; set; }
		public string ImgUri { get; set; }
	}
}
