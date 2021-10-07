using Functional.Maybe;
using JwtAuthApi.Dto.Database;

namespace JwtAuthApi.Ports
{
	public interface IJwtTokenProvider
	{
		string GenerateJwtTokenFor(DataBaseUser dbUser);
		string GenerateRefreshToken();
	}
}