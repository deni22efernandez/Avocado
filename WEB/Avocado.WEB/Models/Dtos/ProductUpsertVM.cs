using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Models.Dtos
{
	public class ProductUpsertVM
	{
		public Product Product { get; set; }
		public IEnumerable<SelectListItem> Categories { get; set; }
	}
}
