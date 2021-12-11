using Avocado.WEB.Models;
using Avocado.WEB.Models.ViewModels;
using Avocado.WEB.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Avocado.WEB.Controllers
{
	[AllowAnonymous]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		
		private readonly IProductRepository _prodRepo;
		private readonly ICategoryRepository _categoryRepo;

		public HomeController(ILogger<HomeController> logger,  IProductRepository prodRepo, ICategoryRepository categoryRepo)
		{
			_logger = logger;
			_prodRepo = prodRepo;
			_categoryRepo = categoryRepo;
		}
		private string GetToken()
		{
			return User.Identity.IsAuthenticated ? ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name).Value : "";
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
			ShoppingCartVM shoppingCartVM = new ShoppingCartVM
			{
				Product = await _prodRepo.GetAsync(id, Common.Common.ProductApi, GetToken())

			};
			return View(shoppingCartVM);
		}
		//[HttpPost]
		//public async Task<IActionResult> DetailsPost(ShoppingCartVM shoppingCartVM)
		//{

		//}

		
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
