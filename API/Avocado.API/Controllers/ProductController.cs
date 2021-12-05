using Avocado.API.DataAccess;
using Avocado.API.Mapper;
using Avocado.API.Models;
using Avocado.API.Models.Dtos.ProductDtos;
using Avocado.API.Repository;
using Avocado.API.Repository.IRepository;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Controllers
{
	[Route("products")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		public ProductController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			var prodList = await _unitOfWork.ProductRepository.GetAllAsync();
			if (prodList.Count() != 0)
			{
				return Ok(prodList.Map<IEnumerable<ProductDto>>());
			}
			return NotFound();
		}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetAsync(int id)
		{
			var prod = await _unitOfWork.ProductRepository.GetAsync(x=>x.Id==id,includeProperties:"Category");
			
			if (prod != null)
			{
				return Ok(prod.Map<ProductDto>());
			}
			return NotFound();
		}
		[HttpPost]
		public async Task<IActionResult> PostAsync([FromBody] ProductCreateDto productDto)
		{
			var objToCreate = productDto.Map<Product>();
			await _unitOfWork.ProductRepository.AddAsync(objToCreate);
			await _unitOfWork.SaveAsync();
			return CreatedAtAction("Get", new { id = objToCreate.Id }, objToCreate);
		}

		[HttpPatch]
		public async Task<IActionResult> Patch([FromBody] ProductUpdateDto productDto)
		{
			if (productDto.Id != 0)//update
			{
				await _unitOfWork.ProductRepository.UpdateAsync(productDto.Map<Product>());
				await _unitOfWork.SaveAsync();
				return NoContent();
			}
			//create
			var objToCreate = productDto.Map<Product>();
			await _unitOfWork.ProductRepository.AddAsync(objToCreate);
			await _unitOfWork.SaveAsync();
			return CreatedAtAction("Get", new { id = objToCreate.Id }, objToCreate);
		}
		[HttpPut]
		public async Task<IActionResult> UpdateAsync([FromBody] ProductUpdateDto productUpdateDto)
		{
			var parameter = new DynamicParameters();
			parameter.Add("@ProdId", productUpdateDto.Id);
			parameter.Add("@Name", productUpdateDto.Name);
			parameter.Add("@Price", productUpdateDto.Price);
			parameter.Add("@Desc", productUpdateDto.Description);
			parameter.Add("@CategoryId", productUpdateDto.CategoryId);
			parameter.Add("@Img", productUpdateDto.ImgUri);

			_unitOfWork.Stored_Proc_Calls.Execute("_sp_UpdateProduct", parameter);
			await _unitOfWork.SaveAsync();
			return NoContent();
		}
	}
}
