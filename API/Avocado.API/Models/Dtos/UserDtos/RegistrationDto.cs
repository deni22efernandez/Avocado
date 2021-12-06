using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Models.Dtos.UserDtos
{
	public class RegistrationDto
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Name { get; set; }
		public string LastName { get; set; }
		public string FullName { get { return this.Name + ", " + this.LastName; } }
		public string PhoneNumber { get; set; }
		public int PostalCode { get; set; }
		public string StreetAddress { get; set; }
		public string City { get; set; }
		public string State { get; set; }
	}
}
