﻿using Avocado.WEB.Common;
using Avocado.WEB.Models;
using Avocado.WEB.Models.ViewModels;
using Avocado.WEB.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductRepository _prodRepo;
		private readonly ICategoryRepository _catRepo;
		private IWebHostEnvironment _hostEnvironment;
		public ProductController(IProductRepository prodRepo, ICategoryRepository catRepo, IWebHostEnvironment hostEnvironment)
		{
			_prodRepo = prodRepo;
			_catRepo = catRepo;
			_hostEnvironment = hostEnvironment;
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
				var webPath = _hostEnvironment.WebRootPath;
				var files = HttpContext.Request.Form.Files;
				string fileName = Guid.NewGuid().ToString();
				var uploads = webPath + @"\images\";
				

				if (product.Id == 0)//create
				{
					if (files.Count() > 0)
					{
						var extention = Path.GetExtension(files[0].FileName);
						using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extention), FileMode.Create))
						{

							files[0].CopyTo(fileStream);
						}
						product.ImgUri = fileName + extention;

					}
					var created = await _prodRepo.PostAsync(product, Common.Common.ProductApi);
					if (created)
					{
						return RedirectToAction(nameof(Index));
					}
					ModelState.AddModelError("error", "Error while creating");
					return RedirectToAction(nameof(Index));
				}
				else
				{
					var objFromDb = await _prodRepo.GetAsync(product.Id, Common.Common.ProductApi);

					if (files.Count() > 0)//if image was uploaded
					{
						var extention = Path.GetExtension(files[0].FileName);
						if (objFromDb.ImgUri != null)
						{
							var imgPath = Path.Combine(uploads, objFromDb.ImgUri);

							if (System.IO.File.Exists(imgPath))
							{
								System.IO.File.Delete(imgPath);
							}
						}
						using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extention), FileMode.Create))
						{
							files[0].CopyTo(fileStream);
						}
						product.ImgUri = fileName + extention;

					}
					else//if no file was uploaded
					{
						product.ImgUri = objFromDb.ImgUri;
					}					
					if (await _prodRepo.PutAsync(product, Common.Common.ProductApi))//update
					{
						//success msg
						return RedirectToAction(nameof(Index));
					}
				}				
			}
			return View(product);
		}
	}
}
