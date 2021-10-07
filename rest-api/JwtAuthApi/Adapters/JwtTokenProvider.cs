using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtAuthApi.Commands;
using JwtAuthApi.Dto.Database;
using JwtAuthApi.Ports;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthApi.Adapters
{
	public class JwtTokenProvider : IJwtTokenProvider
	{
		private readonly IOptions<AuthTokenOptions> _options;

		public JwtTokenProvider(IOptions<AuthTokenOptions> options)
		{
			_options = options;
		}
		
		public string GenerateJwtTokenFor(DataBaseUser dbUser)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.ASCII.GetBytes(_options.Value.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim("Id", dbUser.Id.ToString()),
					new Claim("Email", dbUser.Email)
				}),
				Expires = DateTime.UtcNow.Add(_options.Value.AuthTokenTtl),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(tokenKey),
					SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
        }

		public string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}
	}
}