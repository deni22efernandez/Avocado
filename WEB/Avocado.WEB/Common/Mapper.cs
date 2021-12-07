using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Common
{
	public static class Mapper
	{
		public static T Map<T>(this object value)
		{
			return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(value));
		}
	}
}
