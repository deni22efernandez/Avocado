using Avocado.WEB.Models;
using Avocado.WEB.Models.ViewModels;
using Avocado.WEB.Repository.IRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Avocado.WEB.Repository
{
	public class UserRepository:Repository<User>, IUserRepository
	{
		private readonly IHttpClientFactory _httpClient;
		public UserRepository(IHttpClientFactory httpClient):base(httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<User> LoginAsync(LoginModel loginModel)
		{
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Common.Common.UserApi + "authenticate");
			request.Content = new StringContent(JsonConvert.SerializeObject(loginModel));
			using(var client = _httpClient.CreateClient())
			{
				var response = await client.SendAsync(request);
				if (response.IsSuccessStatusCode)
				{
					string responseContent = await response.Content.ReadAsStringAsync();
					var user = JsonConvert.DeserializeObject<User>(responseContent);
					return user;
				}
				return null;
			}
		}
	}
}
