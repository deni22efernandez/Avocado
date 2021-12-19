using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.SMTP
{
	public class EmailSenderConfig
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string Smtp { get; set; }
		public int Port { get; set; }
		public string From { get; set; }
	}
}
