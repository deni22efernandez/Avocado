using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Repository.IRepository
{
	public interface IRepository<T> where T:class
	{
		Task<T> GetAsync(int id, string uri);
		Task<IEnumerable<T>> GetAllAsync(string uri);
		Task<bool> PostAsync(T entity, string uri);
		Task<bool> PutAsync(T entity, string uri);
	}
}
