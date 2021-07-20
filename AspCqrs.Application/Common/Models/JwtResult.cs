namespace AspCqrs.Application.Common.Models
{
    public class JwtResult
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}