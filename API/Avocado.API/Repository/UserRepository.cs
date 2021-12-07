using Avocado.API.DataAccess;
using Avocado.API.Models;
using Avocado.API.Models.Dtos.UserDtos;
using Avocado.API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Avocado.API.Repository
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly AppSettings _appSettings;
		public UserRepository(ApplicationDbContext context, IOptions<AppSettings> appSettings) : base(context)
		{
			_context = context;
			_appSettings = appSettings.Value;
		}
		public async Task<User> AuthenticateAsync(string username, string password)
		{
			var userFromDb = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username && x.Password == password);
			if (userFromDb == null)
			{
				return null;
			}
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] {
					new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
					new Claim(ClaimTypes.Role, "Customer")
				}),
				Expires = DateTime.UtcNow.AddDays(3),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			userFromDb.Token = tokenHandler.WriteToken(token);
			userFromDb.Password = "";
			return userFromDb;

		}
		public bool IsUnique(string username)
		{
			if (_context.Users.Any(x => x.UserName == username))
			{
				return false;
			}
			return true;
		}

	}
}
