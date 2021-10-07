using System.Collections.Generic;
using JwtAuthApi.Controllers;

namespace JwtAuthApi.Dto.Outbound
{
	public class GetCustomersResponseBody
	{
		public IEnumerable<Customer> Customers { get; set; }
	}

	public class Customer
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
	}
}