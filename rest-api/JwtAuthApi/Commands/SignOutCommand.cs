using System;
using JwtAuthApi.Dto.Database;
using JwtAuthApi.Ports;

namespace JwtAuthApi.Commands
{
	public class SignOutCommand : ICommand
	{
		private readonly Guid _userId;
		private readonly IInMemoryDatabase _database;
		private readonly IProcessingInProgress _processingInProgress;

		public SignOutCommand(
			Guid userId,
			IInMemoryDatabase database,
			IProcessingInProgress processingInProgress)
		{
			_userId = userId;
			_database = database;
			_processingInProgress = processingInProgress;
		}

		public void Handle()
		{
			var user = _database.GetUserById(_userId);

			if (user.HasValue)
			{
				ClearUserRefreshToken(user.Value);
				_processingInProgress.UserSignedOut();
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
	}
}