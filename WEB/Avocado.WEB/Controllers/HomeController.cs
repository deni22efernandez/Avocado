﻿using Avocado.WEB.Models;
using Avocado.WEB.Models.ViewModels;
using Avocado.WEB.Repository.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
	[AllowAnonymous]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUserRepository _userRepo;
		private readonly IProductRepository _prodRepo;
		private readonly ICategoryRepository _categoryRepo;

		public HomeController(ILogger<HomeController> logger, IUserRepository userRepo, IProductRepository prodRepo, ICategoryRepository categoryRepo)
		{
			_logger = logger;
			_userRepo = userRepo;
			_prodRepo = prodRepo;
			_categoryRepo = categoryRepo;
		}

		public async Task<IActionResult> Index()
		{
			HomeIndexVM homeIndexVM = new HomeIndexVM
			{
				Products = await _prodRepo.GetAllAsync(Common.Common.ProductApi, GetToken()),
				Categories = await _categoryRepo.GetAllAsync(Common.Common.CategoryApi, GetToken())
			};
			return View(homeIndexVM);
		}
		public async Task<IActionResult> Details(int id)
		{
			return View(await _prodRepo.GetAsync(id,Common.Common.ProductApi, GetToken()));
		}
		private string GetToken()
		{
			return User.Identity.IsAuthenticated ? ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name).Value : "";
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
				claimIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
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
				user.Role = "customer";
				if(await _userRepo.PostAsync(user, Common.Common.UserApi + "register"))
				{
					return RedirectToAction(nameof(Index));
				}
				return RedirectToAction(nameof(Login));//user already exists
			}
			return View();
		}
	
		[ActionName("Logout")]
		public async Task<IActionResult> LogoutAsync()
		{
			await HttpContext.SignOutAsync();
			return RedirectToAction(nameof(Index));
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
