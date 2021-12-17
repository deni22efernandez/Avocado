using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Common
{
	public static class Common
	{
		private static string BaseUri= "https://localhost:44363";
		public static string ProductApi = BaseUri + "/products/";
		public static string CategoryApi = BaseUri + "/categories/";
		public static string UserApi = BaseUri + "/users/";
		public static string OrderHeaderApi = BaseUri + "/orderHeaders/";
		public static string OrderDetailApi = BaseUri + "/orderDetails/";
	}
}
