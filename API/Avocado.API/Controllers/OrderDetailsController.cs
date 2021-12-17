using Avocado.API.Mapper;
using Avocado.API.Models;
using Avocado.API.Models.Dtos.OrderDetails;
using Avocado.API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Controllers
{
	[Route("orderDetails")]
	[ApiController]
	public class OrderDetailsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		public OrderDetailsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetAsync(int id)
		{
			var orderDetail = await _unitOfWork.OrderDetailRepository.GetAsync(x => x.Id == id);
			if (orderDetail != null)
			{
				return Ok((orderDetail).Map<OrderDetailsGet>());
			}
			return NotFound();
		}
		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			var orderDetails = await _unitOfWork.OrderDetailRepository.GetAllAsync();
			if (orderDetails != null)
			{
				return Ok((orderDetails).Map<IEnumerable<OrderDetailsGet>>());
			}
			return NotFound();
		}

	}
}
