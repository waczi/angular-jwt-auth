using System;
using Functional.Maybe;
using JwtAuthApi.Dto.Database;
using JwtAuthApi.Ports;

namespace JwtAuthApi.Adapters
{
	public class InMemoryDatabase : IInMemoryDatabase
	{
		private readonly object _lock = new object();
		private DataBaseUser _user = new DataBaseUser
		{
			Id = Guid.Parse("824a0956-9b9a-473f-859d-559e0ffafdf2"),
			Email = "qwerty@gmail.com",
			Password = "qwerty"
		};


		public Maybe<DataBaseUser> GetUserByEmail(string email)
		{
			lock (_lock)
			{
				return email.Equals(_user.Email) ? _user.ToMaybe() : Maybe<DataBaseUser>.Nothing;
			}
		}

		public void UpdateUser(DataBaseUser dbUser)
		{
			lock (_lock)
			{
				_user = dbUser;
			}
		}

		public Maybe<DataBaseUser> GetUserById(Guid id)
		{
			lock (_lock)
			{
				return id.Equals(_user.Id) ? _user.ToMaybe() : Maybe<DataBaseUser>.Nothing;
			}
		}
	}
}