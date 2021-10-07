using Microsoft.AspNetCore.Mvc;

namespace JwtAuthApi.Ports
{
	public interface IProcessingInProgress
	{
		IActionResult ToActionResult();
		void InvalidCredentials();
		void SignedIn(string jwtToken, string refreshToken);
		void UserNotFound();
		void RefreshTokenExpired();
		void InvalidRefreshToken();
		void ReSignedIn(string jwtToken, string refreshToken);
		void UserSignedOut();
	}
}