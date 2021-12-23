using Avocado.WEB.Models;
using Avocado.WEB.Models.ViewModels;
using Avocado.WEB.Repository.IRepository;
using Avocado.WEB.SessionXtension;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Avocado.WEB.Controllers
{
	public class OrderController : Controller
	{
		private readonly IOrderHeaderRepository _orderHeaderRepo;
		private readonly IOrderDetailRepository _orderDetaisRepo;
		public OrderController(IOrderHeaderRepository orderHeaderRepo, IOrderDetailRepository orderDetaisRepo)
		{
			_orderHeaderRepo = orderHeaderRepo;
			_orderDetaisRepo = orderDetaisRepo;
		}
		private string GetToken()
		{
			return User.Identity.IsAuthenticated?(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name)).Value : default;
		}
		public async Task<IActionResult> Index(int currentPage=1)
		{
			var headers = await _orderHeaderRepo.GetAllAsync(Common.Common.OrderHeaderApi, GetToken());
			OrderHeaderIndexVM model = new OrderHeaderIndexVM()
			{
				OrderHeaders = headers.Skip((currentPage-1)*3).Take(3).ToList(),
				PaginationModel = new CustomTagHelper.PaginationModel
				{
					CurrentPage = currentPage,
					ItemsPerPage = 3,
					TotalItems = headers.Count(),
					Uri = "/Order/Index?currentPage=:"

				}
			};
			return View(model);
		}
		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			OrderDetailVM orderDetailVM = new OrderDetailVM()
			{
				OrderHeader = await _orderHeaderRepo.GetAsync(id, Common.Common.OrderHeaderApi, GetToken()),
				OrderDetails = await _orderDetaisRepo.GetAllAsync(Common.Common.OrderDetailApi+id, GetToken())
			};
			return View(orderDetailVM);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(OrderDetailVM model)
		{
			if (ModelState.IsValid)
			{
				await _orderHeaderRepo.PatchAsync(model.OrderHeader, Common.Common.OrderHeaderApi, GetToken());

			}
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> StartProcessing(int id)
		{
			var orderFromDb = await _orderHeaderRepo.GetAsync(id, Common.Common.OrderHeaderApi, GetToken());
			orderFromDb.OrderStatus = "Processing";
			await _orderHeaderRepo.PatchAsync(orderFromDb, Common.Common.OrderHeaderApi, GetToken());			
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> ShipOrder(int id)
		{
			var orderFromDb = await _orderHeaderRepo.GetAsync(id, Common.Common.OrderHeaderApi, GetToken());
			orderFromDb.OrderStatus = "Shipped";
			await _orderHeaderRepo.PatchAsync(orderFromDb, Common.Common.OrderHeaderApi, GetToken());
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> CancelOrder(int id)
		{
			var orderFromDb = await _orderHeaderRepo.GetAsync(id, Common.Common.OrderHeaderApi, GetToken());
			if (orderFromDb.PaymentStatus == "approved")
			{
				var options = new RefundCreateOptions
				{
					Reason = RefundReasons.RequestedByCustomer,
					PaymentIntent = orderFromDb.PaymentIntentId
				};

				var service = new RefundService();
				Refund refund = service.Create(options);

				orderFromDb.OrderStatus = "Cancelled";
				orderFromDb.PaymentStatus = "Refunded";				
			}
			else
			{
				orderFromDb.OrderStatus = "Cancelled";
				orderFromDb.PaymentStatus = "Cancelled";
			}
			await _orderHeaderRepo.PatchAsync(orderFromDb, Common.Common.OrderHeaderApi, GetToken());
			return RedirectToAction(nameof(Index));
		}
	}
}
