namespace JwtAuthApi.Dto.Outbound
{
	public class ReSignInResponseBody
	{
		public string RefreshToken { get; set; }
		public string Token { get; set; }
	}
}