using System;
using JwtAuthApi.Dto.Database;
using JwtAuthApi.Dto.Inbound;
using JwtAuthApi.Ports;
using Microsoft.Extensions.Options;

namespace JwtAuthApi.Commands
{
	public class ReSignInCommand : ICommand
	{
		private readonly IInMemoryDatabase _database;
		private readonly ReSignInRequestBody _requestBody;
		private readonly IProcessingInProgress _processingInProgress;
		private readonly IJwtTokenProvider _jwtTokenProvider;
		private readonly IOptions<AuthTokenOptions> _options;

		public ReSignInCommand(
			IInMemoryDatabase database, 
			ReSignInRequestBody requestBody, 
			IProcessingInProgress processingInProgress, 
			IJwtTokenProvider jwtTokenProvider, 
			IOptions<AuthTokenOptions> options)
		{
			_database = database;
			_requestBody = requestBody;
			_processingInProgress = processingInProgress;
			_jwtTokenProvider = jwtTokenProvider;
			_options = options;
		}

		public void Handle()
		{
			var user = _database.GetUserById(_requestBody.UserId);

			if (user.HasValue)
			{
				if (user.Value.RefreshTokenExpiration < DateTime.UtcNow)
				{
					ClearUserRefreshToken(user.Value);
					_processingInProgress.RefreshTokenExpired();
				}
				else if(user.Value.RefreshToken != _requestBody.RefreshToken)
				{
					_processingInProgress.InvalidRefreshToken();
				}
				else
				{
					var jwtToken = _jwtTokenProvider.GenerateJwtTokenFor(user.Value);
					var refreshToken = _jwtTokenProvider.GenerateRefreshToken();

					UpdateUserRefreshToken(user.Value, refreshToken);
					_processingInProgress.ReSignedIn(jwtToken, refreshToken);
				}
			}
			else
			{
				_processingInProgress.UserNotFound();
			}
		}

		private void ClearUserRefreshToken(DataBaseUser user)
		{
			user.RefreshToken = default;
			user.RefreshTokenExpiration = default;
			_database.UpdateUser(user);
		}
		
		private void UpdateUserRefreshToken(DataBaseUser user, string refreshToken)
		{
			user.RefreshToken = refreshToken;
			user.RefreshTokenExpiration = DateTime.UtcNow.Add(_options.Value.RefreshTokenTtl);
			_database.UpdateUser(user);
		}
	}
}