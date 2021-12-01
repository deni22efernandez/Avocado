using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Avocado.API.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		Task<T> GetAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null);
		Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null);
		Task AddAsync(T entity);
		Task DeleteAsync(int id);
	}
}
