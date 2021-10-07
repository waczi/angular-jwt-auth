using JwtAuthApi.Dto.Outbound;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthApi.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/customers/")]
	public class CustomersController : ControllerBase
	{
		[HttpGet("getall")]
		public IActionResult GetCustomers()
		{
			return new JsonResult(new GetCustomersResponseBody
			{
				Customers = new []
				{
					new Customer {Email = "walter.white@gmail.com", FirstName = "Walter", LastName = "White"},
					new Customer {Email = "jesse.pinkman@gmail.com", FirstName = "Jesse", LastName = "Pinkman"},
					new Customer {Email = "mike.ehrmantraut@gmail.com", FirstName = "Mike", LastName = "Ehrmantraut"}
				}
			});
		}
	}
}
