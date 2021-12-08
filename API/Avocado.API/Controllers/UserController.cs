using Avocado.API.Mapper;
using Avocado.API.Models;
using Avocado.API.Models.Dtos.UserDtos;
using Avocado.API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Controllers
{
	[Route("users")]
	[ApiController]
	[AllowAnonymous]
	public class UserController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		public UserController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		[HttpPost("register")]
		public async Task<IActionResult> RegisterAsync([FromBody]RegistrationDto registrationModel)
		{
			if (_unitOfWork.UserRepository.IsUnique(registrationModel.UserName))
			{
				await _unitOfWork.UserRepository.AddAsync(registrationModel.Map<User>());
				await _unitOfWork.SaveAsync();
				return Ok();
			}
			return BadRequest("user already exists!");
		}
		[HttpPost("authenticate")]
		public async Task<IActionResult> LoginAsync([FromBody]LoginModel loginModel)
		{
			var user = await _unitOfWork.UserRepository.AuthenticateAsync(loginModel.UserName, loginModel.Password);
			if (user != null)
			{
				return Ok(user);
			}
			return BadRequest("Username or Passsword incorrect!");
		}
	}
}
