using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Avocado.API.Mapper
{
	public static class CustomMapper
	{
		public static T Map<T>(this object value)
		{
			return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value));
		}
	}
}
