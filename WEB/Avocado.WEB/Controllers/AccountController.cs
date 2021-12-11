using Avocado.WEB.Models;
using Avocado.WEB.Models.ViewModels;
using Avocado.WEB.Repository.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Avocado.WEB.Controllers
{
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
				if (await _userRepo.PostAsync(user, Common.Common.UserApi + "register"))
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
		public IActionResult Index()
		{
			return View();
		}
	}
}
