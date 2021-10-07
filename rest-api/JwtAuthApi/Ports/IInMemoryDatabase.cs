using System;
using Functional.Maybe;
using JwtAuthApi.Dto.Database;

namespace JwtAuthApi.Ports
{
	public interface IInMemoryDatabase
	{
		Maybe<DataBaseUser> GetUserByEmail(string email);
		void UpdateUser(DataBaseUser dbUser);
		Maybe<DataBaseUser> GetUserById(Guid id);
	}
}