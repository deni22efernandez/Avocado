using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.CustomTagHelper
{
	public class PaginationModel
	{
		public int TotalItems { get; set; }
		public int ItemsPerPage { get; set; }
		public int TotalPages { get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); } }
		public string Uri { get; set; }
		public int CurrentPage { get; set; }
	}
}
