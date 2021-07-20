using System;

namespace AspCqrs.Application.Options
{
    public class JwtSettings
    {
        public static readonly string Section = "JwtSettings";
        
        public string Key { get; set; }

        public TimeSpan LifeTime { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}