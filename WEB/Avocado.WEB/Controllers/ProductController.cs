using Avocado.WEB.Models;
using Avocado.WEB.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductRepository _repo;
		public ProductController(IProductRepository repo)
		{
			_repo = repo;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _repo.GetAllAsync(Common.Common.ProductApi));
		}
	}
}
