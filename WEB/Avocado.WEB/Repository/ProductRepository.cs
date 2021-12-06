using Avocado.WEB.Models;
using Avocado.WEB.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Avocado.WEB.Repository
{
	public class ProductRepository:Repository<Product>, IProductRepository
	{
		private readonly IHttpClientFactory _httpClient;
		public ProductRepository(IHttpClientFactory httpClient):base(httpClient)
		{
			_httpClient = httpClient;
		}
	}
}
