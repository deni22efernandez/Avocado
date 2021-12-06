using Avocado.API.Models;
using Avocado.API.Models.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Repository.IRepository
{
	public interface IUserRepository:IRepository<User>
	{
		Task<User> AuthenticateAsync(string username, string password);
		bool IsUnique(string username);
	}
}
