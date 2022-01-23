using Avocado.WEB.Models;
using Avocado.WEB.Models.ViewModels;
using Avocado.WEB.Repository.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Avocado.WEB.Controllers
{
	[AllowAnonymous]
	public class AccountController : Controller
	{
		private readonly IUserRepository _userRepo;
		public AccountController(IUserRepository userRepo)
		{
			_userRepo = userRepo;
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
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LoginAsync(LoginModel loginModel,string returnUrl="")
		{
			if (ModelState.IsValid)
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
					if (!string.IsNullOrEmpty(returnUrl))
					{
						if (Url.IsLocalUrl(returnUrl))
						{
							return LocalRedirect(returnUrl);
						}
					}
					TempData["success"] = $"Welcome {user.UserName}!";
					return RedirectToAction("Index", "Home");
				}
				TempData["error"] = "Incorrect username or password!";
				ModelState.AddModelError("error", "incorrect username or password");
				return View(loginModel);
			}
			TempData["error"] = "Username and password are required!";
			return View();
			
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
				if (await _userRepo.PostAsync(user, Common.Common.UserApi + "register") != null)
				{
					TempData["success"] = $"Successfull registration!";
					return RedirectToAction(nameof(Login));
				}					
				else
				{
					TempData["error"] = "Username already exists!";
					return View(user);
				}
			}
			TempData["error"] = "Username and password are required!";
			return View();
		}

		[ActionName("Logout")]
		public async Task<IActionResult> LogoutAsync()
		{
			HttpContext.Session.Clear();
			await HttpContext.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
