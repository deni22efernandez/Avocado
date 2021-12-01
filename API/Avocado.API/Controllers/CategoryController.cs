using Avocado.API.Mapper;
using Avocado.API.Models.Dtos;
using Avocado.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
			var result = await _unit.CategoryRepository.Get(id);
			return Ok(result.Map<CategoryDto>());
		}
		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			var result = await _unit.CategoryRepository.GetAll();
			return Ok(result.Map<IEnumerable<CategoryDto>>());
		}
	}
}
