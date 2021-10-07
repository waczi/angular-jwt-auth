using Functional.Maybe;
using JwtAuthApi.Dto.Outbound;
using JwtAuthApi.Ports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthApi.Adapters
{
	public class ProcessingInProgress : IProcessingInProgress
	{
		private Maybe<IActionResult> _actionResult = Maybe<IActionResult>.Nothing;
		
		public IActionResult ToActionResult()
		{
			return _actionResult.Value;
		}

		public void InvalidCredentials()
		{
			_actionResult = new JsonResult(new BadRequestResponseBody
			{
				Status = BadRequestStatus.InvalidUserCredentials
			})
			{
				StatusCode = StatusCodes.Status400BadRequest
			}.ToMaybe<IActionResult>();
		}

		public void SignedIn(string jwtToken, string refreshToken)
		{
			_actionResult = new JsonResult(new SignInResponseBody
			{
				RefreshToken = refreshToken,
				Token = jwtToken
			})
			{
				StatusCode = StatusCodes.Status200OK
			}.ToMaybe<IActionResult>();
		}

		public void UserNotFound()
		{
			_actionResult = new JsonResult(new BadRequestResponseBody
			{
				Status = BadRequestStatus.UserNotFound
			})
			{
				StatusCode = StatusCodes.Status400BadRequest
			}.ToMaybe<IActionResult>();
		}

		public void RefreshTokenExpired()
		{
			_actionResult = new JsonResult(new BadRequestResponseBody
			{
				Status = BadRequestStatus.RefreshTokenExpired
			})
			{
				StatusCode = StatusCodes.Status400BadRequest
			}.ToMaybe<IActionResult>();
		}

		public void InvalidRefreshToken()
		{
			_actionResult = new JsonResult(new BadRequestResponseBody
			{
				Status = BadRequestStatus.InvalidRefreshToken
			})
			{
				StatusCode = StatusCodes.Status400BadRequest
			}.ToMaybe<IActionResult>();
		}

		public void ReSignedIn(string jwtToken, string refreshToken)
		{
			_actionResult = new JsonResult(new ReSignInResponseBody
			{
				RefreshToken = refreshToken,
				Token = jwtToken
			})
			{
				StatusCode = StatusCodes.Status200OK
			}.ToMaybe<IActionResult>();
		}

		public void UserSignedOut()
		{
			_actionResult = new StatusCodeResult(200).ToMaybe<IActionResult>();
		}
	}
}