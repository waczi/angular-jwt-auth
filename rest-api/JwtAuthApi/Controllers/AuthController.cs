using System;
using System.Security.Claims;
using JwtAuthApi.Commands;
using JwtAuthApi.Dto.Inbound;
using JwtAuthApi.Ports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthApi.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/auth/")]
	public class AuthController : ControllerBase
	{
		private readonly ICommandFactory _commandFactory;
		private readonly IProcessingInProgress _processingInProgress;

		public AuthController(
			ICommandFactory commandFactory, 
			IProcessingInProgress processingInProgress)
		{
			_commandFactory = commandFactory;
			_processingInProgress = processingInProgress;
		}

		[AllowAnonymous]
		[HttpPost("signin")]
		public IActionResult SignIn(SignInRequestBody httpBody)
		{
			_commandFactory
				.CreateForSignIn(httpBody, _processingInProgress)
				.Handle();

			return _processingInProgress.ToActionResult();
		}

		[AllowAnonymous]
		[HttpPost("resignin")]
		public IActionResult ReSignIn(ReSignInRequestBody httpBody)
		{
			_commandFactory
				.CreateForReSignIn(httpBody, _processingInProgress)
				.Handle();

			return _processingInProgress.ToActionResult();
		}

		[HttpPost("signout")]
		public IActionResult SignOut()
		{
			_commandFactory
				.CreateForSignOut(
					Guid.Parse((HttpContext.User.Identity as ClaimsIdentity)!
						.FindFirst("id").Value),
					_processingInProgress) 
				.Handle();

			return _processingInProgress.ToActionResult();
		}
	}
}
