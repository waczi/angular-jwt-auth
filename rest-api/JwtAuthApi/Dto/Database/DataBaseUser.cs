using System;

namespace JwtAuthApi.Dto.Database
{
	public class DataBaseUser
	{
		public Guid Id { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string RefreshToken { get; set; }
		public DateTime RefreshTokenExpiration { get; set; }
	}
}