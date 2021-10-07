using System;
using JwtAuthApi.Dto.Database;
using JwtAuthApi.Dto.Inbound;
using JwtAuthApi.Ports;
using Microsoft.Extensions.Options;

namespace JwtAuthApi.Commands
{
	public class SignInCommand : ICommand
	{
		private readonly IInMemoryDatabase _database;
		private readonly SignInRequestBody _requestBody;
		private readonly IProcessingInProgress _processingInProgress;
		private readonly IJwtTokenProvider _jwtTokenProvider;
		private readonly IOptions<AuthTokenOptions> _options;

		public SignInCommand(
			IInMemoryDatabase database,
			SignInRequestBody requestBody,
			IProcessingInProgress processingInProgress,
			IJwtTokenProvider jwtTokenProvider,
			IOptions<AuthTokenOptions> options)
		{
			_database = database;
			_requestBody = requestBody;
			_processingInProgress = processingInProgress;
			_options = options;
			_jwtTokenProvider = jwtTokenProvider;
		}

		public void Handle()
		{
			var user = _database.GetUserByEmail(_requestBody.Email);

			if (user.HasValue && user.Value.Password.Equals(_requestBody.Password))
			{
				var jwtToken = _jwtTokenProvider.GenerateJwtTokenFor(user.Value);
				var refreshToken = _jwtTokenProvider.GenerateRefreshToken();

				UpdateUserRefreshToken(user.Value, refreshToken);
				_processingInProgress.SignedIn(jwtToken, refreshToken);
			}
			else
			{
				_processingInProgress.InvalidCredentials();
			}
		}
		
		private void UpdateUserRefreshToken(DataBaseUser user, string refreshToken)
		{
			user.RefreshToken = refreshToken;
			user.RefreshTokenExpiration = DateTime.UtcNow.Add(_options.Value.RefreshTokenTtl);
			_database.UpdateUser(user);
		}
	}
}