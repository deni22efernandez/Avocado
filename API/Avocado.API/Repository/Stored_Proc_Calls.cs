using Avocado.API.DataAccess;
using Avocado.API.Repository.IRepository;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Repository
{
	public class Stored_Proc_Calls:IStored_Proc_Calls
	{
		private readonly ApplicationDbContext _db;
		public string ConnectionString ="";
		public Stored_Proc_Calls(ApplicationDbContext db)
		{
			_db = db;
			ConnectionString = _db.Database.GetDbConnection().ConnectionString;
		}
		public void Execute(string procedureName, DynamicParameters param = null)
		{
			using(var sqlConn= new SqlConnection(ConnectionString))
			{
				sqlConn.Open();
				sqlConn.Execute(procedureName, param, commandType:System.Data.CommandType.StoredProcedure);
			}
		}
	}
}
