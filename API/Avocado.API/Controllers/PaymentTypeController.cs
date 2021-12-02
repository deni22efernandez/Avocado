using Avocado.API.Mapper;
using Avocado.API.Models.Dtos;
using Avocado.API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.Controllers
{
	[Route("paymentTypes")]
	[ApiController]
	public class PaymentTypeController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		public PaymentTypeController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		// GET: api/<PaymentTypeController>
		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			var paymentTypes = await _unitOfWork.PaymentTypeRepository.GetAllAsync();
			return Ok(paymentTypes.Map<IEnumerable<PaymentTypeDto>>());
		}

		// GET api/<PaymentTypeController>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetAsync(int id)
		{
			var paymentType = await _unitOfWork.PaymentTypeRepository.GetAsync(x => x.Id == id);
			return Ok(paymentType.Map<PaymentTypeDto>());
		}

		//// POST api/<PaymentTypeController>
		//[HttpPost]
		//public void Post([FromBody] string value)
		//{
		//}

		//// PUT api/<PaymentTypeController>/5
		//[HttpPut("{id}")]
		//public void Put(int id, [FromBody] string value)
		//{
		//}

		//// DELETE api/<PaymentTypeController>/5
		//[HttpDelete("{id}")]
		//public void Delete(int id)
		//{
		//}
	}
}
