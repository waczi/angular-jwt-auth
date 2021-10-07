using System;

namespace JwtAuthApi.Dto.Inbound
{
	public class ReSignInRequestBody
	{
		public string RefreshToken { get; set; }
		public Guid UserId { get; set; }
	}
}