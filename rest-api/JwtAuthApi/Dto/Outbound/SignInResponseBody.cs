namespace JwtAuthApi.Dto.Outbound
{
	public class SignInResponseBody
	{
		public string Token { get; set; }
		public string RefreshToken { get; set; }
	}
}
