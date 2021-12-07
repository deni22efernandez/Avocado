using Avocado.WEB.Models;
using Avocado.WEB.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Avocado.WEB.Repository
{
	public class CategoryRepository:Repository<Category>, ICategoryRepository
	{
		private readonly IHttpClientFactory _clientFactory;
		public CategoryRepository(IHttpClientFactory clientFactory):base(clientFactory)
		{
			_clientFactory = clientFactory;
		}
	}
}
