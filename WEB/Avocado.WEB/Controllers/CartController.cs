using Avocado.WEB.Common;
using Avocado.WEB.Models;
using Avocado.WEB.Models.Dtos;
using Avocado.WEB.Models.ViewModels;
using Avocado.WEB.Repository.IRepository;
using Avocado.WEB.SessionXtension;
using Avocado.WEB.SMTP;
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
		private readonly IEmailSender _emailSender;
		public CartController(IUserRepository userRepo,
			IProductRepository productRepo,
			IOrderHeaderRepository orderHeaderRepo,
			IOrderDetailRepository orderDetailsRepo,
			IEmailSender emailSender)
		{
			_userRepo = userRepo;
			_productRepo = productRepo;
			_orderHeaderRepo = orderHeaderRepo;
			_orderDetailsRepo = orderDetailsRepo;
			_emailSender = emailSender;
		}
		private string GetToken()
		{
			return (((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name)).ToString() ?? "";
		}
		public async Task<IActionResult> Index()
		{
			List<ShoppingCart> carts = new List<ShoppingCart>();
			carts = HttpContext.Session.Get<List<ShoppingCart>>("sessionCart") ?? default;
			if (carts != null)
			{
				IEnumerable<Product> products = await _productRepo.GetAllAsync(Common.Common.ProductApi, GetToken());
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
			var user = await _userRepo.GetAsync(userId, Common.Common.UserApi, GetToken());
			var carts = HttpContext.Session.Get<List<ShoppingCart>>("sessionCart");
			foreach (var item in carts)
			{
				item.Product = await _productRepo.GetAsync(item.ProductId, Common.Common.ProductApi, GetToken());
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
				var result = await _orderHeaderRepo.PostAsync(orderHeader, Common.Common.OrderHeaderApi, GetToken());
				orderHeader.Id = result.Id;
				foreach (var item in summaryVM.cartItems)
				{
					OrderDetail orderDetail = new OrderDetail
					{
						OrderHeaderId = orderHeader.Id,
						ProductId = item.ProductId,
						Count = item.Count
					};
					await _orderDetailsRepo.PostAsync(orderDetail, Common.Common.OrderDetailApi, GetToken());					
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
				await _orderHeaderRepo.PatchAsync(orderHeader, Common.Common.OrderHeaderApi);//patch
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
		public async Task<IActionResult> OrderConfirmation(int id)
		{
			var order = await _orderHeaderRepo.GetAsync(id, Common.Common.OrderHeaderApi, GetToken());
			if (order != null && order.PaymentStatus!= "ApprovedForDelayedPayment")
			{
				var service = new SessionService();
				Session session = service.Get(order.SessionId);
				if (session.PaymentStatus.ToLower() == "paid")
				{
					order.OrderStatus = "approved";
					order.PaymentStatus = "approved";
					await _orderHeaderRepo.PatchAsync(order, Common.Common.OrderHeaderApi, GetToken());//patch
					//send email
					await _emailSender.SendEmailAsync(order.Email, "Order confirmation", $"Payment for order #{order.Id} has been approved");
					HttpContext.Session.Clear();
				}
				return View(order.Id);
			}
				
			else
				return NotFound();
		}
	}


}
