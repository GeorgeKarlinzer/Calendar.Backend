namespace Calendar.Shared.Abstractions.Auth
{
    public class JwtOptions
    {
        public TimeSpan AccessTokenExpiry { get; set; }
        public TimeSpan RefreshTokenExpiry { get; set; }
        public string AccessTokenCookieName { get; set; }
        public string RefreshTokenCookieName { get; set; }
    }
}