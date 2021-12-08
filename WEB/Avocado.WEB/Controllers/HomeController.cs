using Avocado.WEB.Models;
using Avocado.WEB.Models.ViewModels;
using Avocado.WEB.Repository.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Avocado.WEB.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUserRepository _userRepo;

		public HomeController(ILogger<HomeController> logger, IUserRepository userRepo)
		{
			_logger = logger;
			_userRepo = userRepo;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> LoginAsync(LoginModel loginModel)
		{
			var user = await _userRepo.LoginAsync(loginModel);
			if (user != null)
			{
				var claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
				claimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
				claimIdentity.AddClaim(new Claim(ClaimTypes.Email, user.UserName));
				claimIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Token));
				ClaimsPrincipal principal = new ClaimsPrincipal(claimIdentity);
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
				return RedirectToAction(nameof(Index));
			}
			ModelState.AddModelError("error", "incorrect username or password");
			return View(loginModel);
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View(new User());
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RegisterAsync(User user)
		{
			if (ModelState.IsValid)
			{
				if(await _userRepo.PostAsync(user, Common.Common.UserApi + "register"))
				{
					return RedirectToAction(nameof(Index));
				}
				return RedirectToAction(nameof(Login));//user already exists
			}
			return View();
		}
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
