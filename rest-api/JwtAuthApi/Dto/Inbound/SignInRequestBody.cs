namespace JwtAuthApi.Dto.Inbound
{
	public class SignInRequestBody
	{
		public string Password { get; set; }
		
		public string Email { get; set; }
	}
}