using Avocado.WEB.Models;
using Avocado.WEB.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Repository.IRepository
{
	public interface IUserRepository:IRepository<User>
	{
		Task<User> LoginAsync(LoginModel loginModel);
	}
}
