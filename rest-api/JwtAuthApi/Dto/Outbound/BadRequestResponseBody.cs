using System.Text.Json.Serialization;

namespace JwtAuthApi.Dto.Outbound
{
	public enum BadRequestStatus
	{
		UserNotFound,
		RefreshTokenExpired,
		InvalidRefreshToken,
		InvalidUserCredentials
	}

	public class BadRequestResponseBody
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public BadRequestStatus Status { get; set; }
	}
}