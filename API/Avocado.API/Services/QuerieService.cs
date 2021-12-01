using Avocado.API.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Services
{
	public interface IQuerieService<T> where T : class
	{
		Task<T> Get(int id);
		Task<IEnumerable<T>> GetAll();
	}
	public class QuerieService<T> : IQuerieService<T> where T : class
	{
		private readonly ApplicationDbContext _context;
	    internal DbSet<T> _dbSet;
		public QuerieService(ApplicationDbContext context)
		{
			_context = context;
			_dbSet = _context.Set<T>();
		}

		public async Task<T> Get(int id)
		{
			var obj = await _dbSet.FindAsync(id);
			if (obj == null)
			{
				throw new Exception("Not found");
			}
			return obj;
		}

		public async Task<IEnumerable<T>> GetAll()
		{
			return await _dbSet.ToListAsync();

		}
	}
}
