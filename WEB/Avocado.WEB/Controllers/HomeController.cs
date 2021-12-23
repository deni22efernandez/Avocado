using Avocado.WEB.Models;
using Avocado.WEB.Models.ViewModels;
using Avocado.WEB.Repository.IRepository;
using Avocado.WEB.SessionXtension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

		private readonly IProductRepository _prodRepo;
		private readonly ICategoryRepository _categoryRepo;

		public HomeController(ILogger<HomeController> logger, IProductRepository prodRepo, ICategoryRepository categoryRepo)
		{
			_logger = logger;
			_prodRepo = prodRepo;
			_categoryRepo = categoryRepo;
		}
		[BindProperty]
		public ShoppingCartVM shoppingCartVM { get; set; }
		private string GetToken()
		{
			return User.Identity.IsAuthenticated ? ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name).Value : "";
		}

		public async Task<IActionResult> Index(int currentPage=1)
		{
			var productList = await _prodRepo.GetAllAsync(Common.Common.ProductApi, GetToken());		

			HomeIndexVM homeIndexVM = new HomeIndexVM
			{
				Products = productList,
				Categories = await _categoryRepo.GetAllAsync(Common.Common.CategoryApi, GetToken())				
			};
			return View(homeIndexVM);
		}
		public async Task<IActionResult> Details(int id)
		{
			shoppingCartVM = new ShoppingCartVM
			{
				Product = await _prodRepo.GetAsync(id, Common.Common.ProductApi, GetToken())
			};
			IEnumerable<ShoppingCart> cart = new List<ShoppingCart>();
			cart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>("sessionCart") ?? default;
			shoppingCartVM.ExistsInCart = cart != null ? cart.Any(x => x.ProductId == id) ? true : false : false;
			return View(shoppingCartVM);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Details()
		{
			if (ModelState.IsValid)
			{							
				var cart = HttpContext.Session.Get<List<ShoppingCart>>("sessionCart") ?? new List<ShoppingCart>();
				ShoppingCart newCart = new ShoppingCart
				{
					Count = shoppingCartVM.Count,
					ProductId = shoppingCartVM.Product.Id
				};
				cart.Add(newCart);
				HttpContext.Session.Set<IEnumerable<ShoppingCart>>("sessionCart", cart);
				return RedirectToAction(nameof(Index));
			}
			return View(shoppingCartVM);
		}

		public IActionResult Remove(int id)
		{
			List<ShoppingCart> cart = new List<ShoppingCart>();
			cart = HttpContext.Session.Get<List<ShoppingCart>>("sessionCart") ?? default;
			if (cart != null)
			{
				cart = cart.Any(x => x.ProductId == id) ? cart.Where(x => x.ProductId != id).ToList() : cart;
				HttpContext.Session.Clear();
				cart = cart.Count() > 0 ? cart : null;
				HttpContext.Session.Set<IEnumerable<ShoppingCart>>("sessionCart", cart);
			}
			//item removed successfully
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
