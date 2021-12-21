using Avocado.WEB.Models;
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
		public OrderController(IOrderHeaderRepository orderHeaderRepo)
		{
			_orderHeaderRepo = orderHeaderRepo;
		}
		private string GetToken()
		{
			return User.Identity.IsAuthenticated?(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name)).Value : default;
		}
		public async Task<IActionResult> Index()
		{

			return View(await _orderHeaderRepo.GetAllAsync(Common.Common.OrderHeaderApi, GetToken()));
		}
	}
}
