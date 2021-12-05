using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Repository.IRepository
{
	public interface IStored_Proc_Calls
	{
		void Execute(string procedureName, DynamicParameters param = null);
		T ExecuteScalar<T>(string procedureName, DynamicParameters param = null);
	}
}
