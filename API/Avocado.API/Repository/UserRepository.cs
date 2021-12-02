using Avocado.API.DataAccess;
using Avocado.API.Models;
using Avocado.API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Repository
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		private readonly ApplicationDbContext _context;
		public UserRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}
		public void Update(User user)
		{
			throw new NotImplementedException();
		}
	}
}
