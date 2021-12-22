using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Avocado.WEB.Repository.IRepository
{
	public interface IRepository<T> where T:class
	{
		Task<T> GetAsync(int id, string uri, string token=null);
		Task<IEnumerable<T>> GetAllAsync(string uri, string token = null);
		Task<T> PostAsync(T entity, string uri, string token = null);
		Task<bool> PutAsync(T entity, string uri, string token = null);
		Task<bool> PatchAsync(T entity, string uri, string token = null);
	}
}
