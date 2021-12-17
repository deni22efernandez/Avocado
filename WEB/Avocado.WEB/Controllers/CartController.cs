using Avocado.WEB.Models;
using Avocado.WEB.Models.ViewModels;
using Avocado.WEB.Repository.IRepository;
using Avocado.WEB.SessionXtension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Avocado.WEB.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		private readonly IUserRepository _userRepo;
		private readonly IProductRepository _productRepo;
		public CartController(IUserRepository userRepo, IProductRepository productRepo)
		{
			_userRepo = userRepo;
			_productRepo = productRepo;
		}
		public async Task<IActionResult> Index()
		{
			List<ShoppingCart> carts = new List<ShoppingCart>();
			carts = HttpContext.Session.Get<List<ShoppingCart>>("sessionCart") ?? default;
			if (carts != null)
			{
				IEnumerable<Product> products = await _productRepo.GetAllAsync(Common.Common.ProductApi);
				foreach (var item in carts)
				{
					item.Product = products.Where(x => x.Id == item.ProductId).FirstOrDefault();
				}
			}
			return View(carts);
		}
		public async Task<IActionResult> Summary()
		{
			var userId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
			var user = await _userRepo.GetAsync(userId, Common.Common.UserApi);
			var carts = HttpContext.Session.Get<List<ShoppingCart>>("sessionCart");
			foreach (var item in carts)
			{
				item.Product = await _productRepo.GetAsync(item.ProductId, Common.Common.ProductApi);
			}
			SummaryVM summaryVM = new SummaryVM()
			{
				cartItems = carts,
				Customer = user
			};
			
			return View(summaryVM);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Summary(SummaryVM summaryVM)
		{
			if (ModelState.IsValid)
			{
				double orderTotal = 0;
				foreach (var item in summaryVM.cartItems)
				{
					orderTotal += item.Count * item.Product.Price;
				}

				OrderHeader orderHeader = new OrderHeader()
				{
					OrderDate = DateTime.Now,
					OrderStatus = "pending",
					OrderTotal = orderTotal,
					User = summaryVM.Customer,
					Email = summaryVM.Customer.UserName,
					Name = summaryVM.Customer.Name,
					LastName = summaryVM.Customer.LastName,
					StreetAddress = summaryVM.Customer.StreetAddress,
					City = summaryVM.Customer.City,
					PostalCode = summaryVM.Customer.PostalCode,
					State = summaryVM.Customer.State,
					PhoneNumber = summaryVM.Customer.PhoneNumber

				};
			}
			return View();
		}

		[HttpPost]
		public IActionResult UpdateCart(List<ShoppingCart> carts)
		{
			HttpContext.Session.Get<List<ShoppingCart>>("sessionCart").Clear();
			HttpContext.Session.Set<List<ShoppingCart>>("sessionCart", carts);
			return RedirectToAction(nameof(Index));
		}
		public IActionResult Remove(int id)
		{
			List<ShoppingCart> sessionCarts = new List<ShoppingCart>();
			sessionCarts = HttpContext.Session.Get<List<ShoppingCart>>("sessionCart") ?? default;
			if (sessionCarts.Count > 0)
			{
				var cartToDelete = sessionCarts.FirstOrDefault(x=>x.ProductId == id);
				sessionCarts.Remove(cartToDelete);
				HttpContext.Session.Set<List<ShoppingCart>>("sessionCart", sessionCarts);
				
			}
			return RedirectToAction(nameof(Index));
		}
		public IActionResult Clear()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");
		}
	}

	
}
