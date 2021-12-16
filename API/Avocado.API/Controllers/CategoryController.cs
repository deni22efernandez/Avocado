using Avocado.API.Mapper;
using Avocado.API.Models;
using Avocado.API.Models.Dtos;
using Avocado.API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avocado.API.Controllers
{
	[Route("categories")]
	[ApiController]

	public class CategoryController : ControllerBase
	{
		private readonly IUnitOfWork _unit;
		public CategoryController(IUnitOfWork unit)
		{
			_unit = unit;
		}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetAsync(int id)
		{
			var result = await _unit.CategoryRepository.GetAsync(x => x.Id == id);
			return Ok(result.Map<CategoryDto>());
		}
		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			var result = await _unit.CategoryRepository.GetAllAsync();
			return Ok(result.Map<IEnumerable<CategoryDto>>());
		}
		[HttpPost]
		//[Authorize(Roles = "Admin")]
		public async Task<JsonResult> PostAsync(CategoryCreateDto categoryCreateDto)
		{
			var obj = categoryCreateDto.Map<Category>();
			await _unit.CategoryRepository.AddAsync(obj);
			//await _unit.SaveAsync();
			//return Created("GetAsync", obj);
			JsonResult result = new JsonResult(obj);
			return result;
		}
	}
}
