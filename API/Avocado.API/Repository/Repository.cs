using Avocado.API.DataAccess;
using Avocado.API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Avocado.API.Repository
{
	public class Repository<T> : IRepository<T>  where T : class
	{
		private readonly ApplicationDbContext _context;
	    internal DbSet<T> _dbSet;
		public Repository(ApplicationDbContext context)
		{
			_context = context;
			_dbSet = _context.Set<T>();
		}
		public T Get(Expression<Func<T, bool>> filter = null, string includeProperties = null)
		{
			IQueryable<T> query = _dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(item);
				}
			}

			return query.AsNoTracking().FirstOrDefault();
		}

		public async Task<T> GetAsync(Expression<Func<T,bool>> filter=null, string includeProperties=null)
		{
			IQueryable<T> query = _dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var item in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
				{
					query=query.Include(item);
				}
			}
			
			return await query.AsNoTracking().FirstOrDefaultAsync();	
		}

		
		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);					
		}
		public async Task DeleteAsync(int id)
		{
			var entityFromDb = await _dbSet.FindAsync(id);
			_dbSet.Remove(entityFromDb);
		}

		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null)
		{
			IQueryable<T> query = _dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(item);
				}
			}

			return await query.AsNoTracking().ToListAsync();
		}
	}
}
