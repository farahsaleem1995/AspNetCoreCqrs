namespace AspCqrs.Application.Options
{
    public class RedisSettings
    {
        public const string Section = "RedisSettings";
        
        public string Password { get; set; }

        public bool AllowAdmin { get; set; }

        public bool Ssl { get; set; }

        public int ConnectTimeout { get; set; }

        public int ConnectRetry { get; set; }
        
        public bool AbortOnConnectFail { get; set; }

        public string Connection { get; set; }
    }
}