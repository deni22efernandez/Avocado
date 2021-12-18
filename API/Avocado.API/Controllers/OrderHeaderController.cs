using Avocado.API.DataAccess;
using Avocado.API.Mapper;
using Avocado.API.Models;
using Avocado.API.Models.Dtos.OrderHeader;
using Avocado.API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Controllers
{
	//[Authorize]
	[Route("orderHeaders")]
	[ApiController]
	public class OrderHeaderController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		public OrderHeaderController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetAsync(int id)
		{
			var orderHeader = await _unitOfWork.OrderHeaderRepository.GetAsync(x => x.Id == id);
			if (orderHeader != null)
			{
				return Ok((orderHeader).Map<OrderHeaderGet>());
			}
			return NotFound();
		}
		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			var orderHeaders = await _unitOfWork.OrderHeaderRepository.GetAllAsync();
			if (orderHeaders != null)
			{
				return Ok((orderHeaders).Map<IEnumerable<OrderHeaderGet>>());
			}
			return NotFound();
		}
		[HttpPost]
		public async Task<IActionResult> PostAsync([FromBody]OrderHeaderCreate orderHeader)
		{
			var orderCreated = orderHeader.Map<OrderHeader>();
			await _unitOfWork.OrderHeaderRepository.AddAsync(orderCreated);
			bool result = await _unitOfWork.SaveAsync();
			if (result)
				return CreatedAtRoute("Get", orderHeader.Map<OrderHeader>(), orderCreated);
			else
				return BadRequest();
			
		}
		[HttpPut]
		public async Task<IActionResult> PutAsync([FromBody]OrderHeaderUpdate orderHeaderUpdate)
		{
			await _unitOfWork.OrderHeaderRepository.UpdateAsync(orderHeaderUpdate.Map<OrderHeader>());
			await _unitOfWork.SaveAsync();
			return Ok();
		}
	}
}
