using System;

namespace JwtAuthApi.Commands
{
	public interface ICommand
	{
		void Handle();
	}

	public class AuthTokenOptions
	{
		public const string SectionName = "JwtSettings";
		
		public string Secret { get; set; }
		public TimeSpan AuthTokenTtl { get; set; }
		public TimeSpan RefreshTokenTtl { get; set; }
	}
}