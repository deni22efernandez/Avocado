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
		//[HttpGet("{id:int}")]
		//public IActionResult Get(int id)
		//{
		//	var orderDetail = _unitOfWork.OrderDetailRepository.Get(x => x.Id == id);
		//	if (orderDetail != null)
		//	{
		//		return Ok((orderDetail).Map<OrderDetailsGet>());
		//	}
		//	return NotFound();
		//}
		//[HttpGet]
		//public async Task<IActionResult> GetAsync()
		//{					
		//	var orderDetails = await _unitOfWork.OrderDetailRepository.GetAllAsync();
		//	if (orderDetails != null)
		//	{
		//		return Ok((orderDetails).Map<IEnumerable<OrderDetailsGet>>());
		//	}
		//	return NotFound();
		//}
		[HttpGet("{orderid:int}")]
		public async Task<IActionResult> GetByOrderHeaderId(int orderid)
		{
			var orderDetails = await _unitOfWork.OrderDetailRepository.GetByOrderHeaderId(orderid);
			if (orderDetails != null)
			{
				return Ok(orderDetails);
			}
			return NotFound();
		}
		[HttpPost]
		public async Task<IActionResult> PostAsync([FromBody]OrderDetailsCreate orderDetailsCreate)
		{
			if (orderDetailsCreate != null)
			{
				var created = orderDetailsCreate.Map<OrderDetail>();
				await _unitOfWork.OrderDetailRepository.AddAsync(created);
				await _unitOfWork.SaveAsync();
				return Created("Get", created);
			}
			return BadRequest();
		}
		[HttpPut]
		public async Task<IActionResult> PutAsync([FromBody]OrderDetailsUpdate orderDetailsUpdate)
		{
			if (orderDetailsUpdate != null)
			{
				await _unitOfWork.OrderDetailRepository.UpdateAsync(orderDetailsUpdate.Map<OrderDetail>());
				await _unitOfWork.SaveAsync();
				return NoContent();
			}
			return BadRequest();
		}


	}
}
