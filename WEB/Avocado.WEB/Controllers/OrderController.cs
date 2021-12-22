using Avocado.WEB.Models;
using Avocado.WEB.Models.ViewModels;
using Avocado.WEB.Repository.IRepository;
using Avocado.WEB.SessionXtension;
using Microsoft.AspNetCore.Mvc;
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
	}
}
