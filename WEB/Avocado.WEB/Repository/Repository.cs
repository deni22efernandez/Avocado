using Avocado.WEB.Repository.IRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Avocado.WEB.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly IHttpClientFactory _httpClient;
		public Repository(IHttpClientFactory httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<T> GetAsync(int id, string uri)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, uri+id);
			using (var client = _httpClient.CreateClient())
			{				
				HttpResponseMessage response = await client.SendAsync(request);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					return JsonConvert.DeserializeObject<T>(result);
				}
				return null;
			}
		}
		public async Task<IEnumerable<T>> GetAllAsync(string uri)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, uri);
			using (var client = _httpClient.CreateClient())
			{
				HttpResponseMessage response = await client.SendAsync(request);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					return JsonConvert.DeserializeObject<IEnumerable<T>>(result);
				}
				return null;
			}
		}
		public async Task<bool> PostAsync(T entity, string uri)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, uri);
			request.Content = new StringContent(JsonConvert.SerializeObject(entity), System.Text.Encoding.UTF8, "application/json");
			using (var client = _httpClient.CreateClient())
			{
				HttpResponseMessage response = await client.SendAsync(request);
				if (response.StatusCode==System.Net.HttpStatusCode.Created)
				{
					return true;
				}
				return false;
			}
		}
		public async Task<bool> PutAsync(T entity, string uri)
		{
			var request = new HttpRequestMessage(HttpMethod.Patch, uri);
			request.Content = new StringContent(JsonConvert.SerializeObject(entity), System.Text.Encoding.UTF8, "application/json");
			using (var client = _httpClient.CreateClient())
			{
				HttpResponseMessage response = await client.SendAsync(request);
				if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
				{
					return true;
				}
				return false;
			}
		}
	}
}
