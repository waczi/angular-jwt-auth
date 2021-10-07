using System;
using JwtAuthApi.Dto.Inbound;
using JwtAuthApi.Ports;
using Microsoft.Extensions.Options;

namespace JwtAuthApi.Commands
{
	public interface ICommandFactory
	{
		ICommand CreateForSignIn(SignInRequestBody body, IProcessingInProgress processingInProgress);
		ICommand CreateForReSignIn(ReSignInRequestBody httpBody, IProcessingInProgress processingInProgress);
		ICommand CreateForSignOut(Guid userId, IProcessingInProgress processingInProgress);
	}

	public class CommandFactory : ICommandFactory
	{
		private readonly IInMemoryDatabase _inMemoryDatabase;
		private readonly IJwtTokenProvider _jwtTokenProvider;
		private readonly IOptions<AuthTokenOptions> _options;

		public CommandFactory(
			IInMemoryDatabase inMemoryDatabase, 
			IJwtTokenProvider jwtTokenProvider, 
			IOptions<AuthTokenOptions> options)
		{
			_inMemoryDatabase = inMemoryDatabase;
			_jwtTokenProvider = jwtTokenProvider;
			_options = options;
		}

		public ICommand CreateForSignIn(SignInRequestBody body, IProcessingInProgress processingInProgress)
		{
			return new SignInCommand(
				_inMemoryDatabase,
				body,
				processingInProgress,
				_jwtTokenProvider,
				_options);
		}

		public ICommand CreateForReSignIn(ReSignInRequestBody httpBody, IProcessingInProgress processingInProgress)
		{
			return new ReSignInCommand(
				_inMemoryDatabase,
				httpBody,
				processingInProgress,
				_jwtTokenProvider,
				_options);
		}

		public ICommand CreateForSignOut(Guid userId, IProcessingInProgress processingInProgress)
		{
			return new SignOutCommand(
				userId,
				_inMemoryDatabase, 
				processingInProgress);
		}
	}
}