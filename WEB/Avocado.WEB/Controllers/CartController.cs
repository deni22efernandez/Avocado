using Avocado.WEB.Common;
using Avocado.WEB.Models;
using Avocado.WEB.Models.Dtos;
using Avocado.WEB.Models.ViewModels;
using Avocado.WEB.Repository.IRepository;
using Avocado.WEB.SessionXtension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
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
		private readonly IOrderHeaderRepository _orderHeaderRepo;
		private readonly IOrderDetailRepository _orderDetailsRepo;
		public CartController(IUserRepository userRepo,
			IProductRepository productRepo,
			IOrderHeaderRepository orderHeaderRepo,
			IOrderDetailRepository orderDetailsRepo)
		{
			_userRepo = userRepo;
			_productRepo = productRepo;
			_orderHeaderRepo = orderHeaderRepo;
			_orderDetailsRepo = orderDetailsRepo;
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
				Customer = (user).Map<CustomerDto>()
			};

			return View(summaryVM);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Summary(SummaryVM summaryVM)
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
					PaymentStatus="pending",
					OrderTotal = orderTotal,
					UserId = summaryVM.Customer.Id,
					Email = summaryVM.Customer.UserName,
					Name = summaryVM.Customer.Name,
					LastName = summaryVM.Customer.LastName,
					StreetAddress = summaryVM.Customer.StreetAddress,
					City = summaryVM.Customer.City,
					PostalCode = summaryVM.Customer.PostalCode,
					State = summaryVM.Customer.State,
					PhoneNumber = summaryVM.Customer.PhoneNumber

				};
				await _orderHeaderRepo.PostAsync(orderHeader, Common.Common.OrderHeaderApi);
				foreach (var item in summaryVM.cartItems)
				{
					OrderDetail orderDetail = new OrderDetail
					{
						OrderHeaderId = orderHeader.Id,
						ProductId = item.ProductId,
						Count = item.Count
					};
					await _orderDetailsRepo.PostAsync(orderDetail, Common.Common.OrderDetailApi);					
				}
				//stripe settings 
				var domain = "https://localhost:44348/";
				var options = new SessionCreateOptions
				{
					PaymentMethodTypes = new List<string>
				{
				  "card",
				},
					LineItems = new List<SessionLineItemOptions>(),
					Mode = "payment",
					SuccessUrl = domain + $"cart/OrderConfirmation?id={orderHeader.Id}",
					CancelUrl = domain + $"cart/index",
				};

				foreach (var item in summaryVM.cartItems)
				{

					var sessionLineItem = new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							UnitAmount = (long)(item.Product.Price * 100),//20.00 -> 2000
							Currency = "usd",
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = item.Product.Name
							},

						},
						Quantity = item.Count,
					};
					options.LineItems.Add(sessionLineItem);

				}

				var service = new SessionService();
				Session session = service.Create(options);
				orderHeader.SessionId = session.Id;
				orderHeader.PaymentIntentId = session.PaymentIntentId;
				await _orderHeaderRepo.PutAsync(orderHeader, Common.Common.OrderHeaderApi);
				Response.Headers.Add("Location", session.Url);
				return new StatusCodeResult(303);

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
				var cartToDelete = sessionCarts.FirstOrDefault(x => x.ProductId == id);
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
