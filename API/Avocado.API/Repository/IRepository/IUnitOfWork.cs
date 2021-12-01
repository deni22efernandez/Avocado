using Avocado.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Repository.IRepository
{
	public interface IUnitOfWork : IDisposable
	{
		ICategoryRepository CategoryRepository { get; }
		void Save();
	}
}
