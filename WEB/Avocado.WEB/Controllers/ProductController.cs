using Avocado.WEB.Common;
using Avocado.WEB.Models;
using Avocado.WEB.Models.Dtos;
using Avocado.WEB.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductRepository _prodRepo;
		private readonly ICategoryRepository _catRepo;
		public ProductController(IProductRepository prodRepo, ICategoryRepository catRepo)
		{
			_prodRepo = prodRepo;
			_catRepo = catRepo;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _prodRepo.GetAllAsync(Common.Common.ProductApi));
		}
		[HttpGet]
		public async Task<IActionResult> Upsert(int? id)
		{
			IEnumerable<Category> categories = await _catRepo.GetAllAsync(Common.Common.CategoryApi);
			ProductUpsertVM viewModel = new ProductUpsertVM
			{
				Product = new Product(),
				Categories = categories.Select(x =>
					new SelectListItem
					{
						Text = x.Name,
						Value = x.Id.ToString()
					})
			};
			if (id != null)
			{
				viewModel.Product = await _prodRepo.GetAsync(id.GetValueOrDefault(), Common.Common.ProductApi);
			}
				
			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Upsert(Product product)
		{
			if (ModelState.IsValid)
			{
				if (product.Id == 0)//create
				{
					//save picture
					var created = await _prodRepo.PostAsync(product, Common.Common.ProductApi);
					if (created)
					{
						return RedirectToAction(nameof(Index));
					}
					ModelState.AddModelError("error","Error while creating");
					return RedirectToAction(nameof(Index));
				}
				if(await _prodRepo.PutAsync(product, Common.Common.ProductApi))
				{
					//success msg
					return RedirectToAction(nameof(Index));
				}
				
			}
			return View(product);
		}
	}
}
