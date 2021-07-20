using System;

namespace AspCqrs.Domain.Entities
{
    public class RefreshToken
    {
        public string JwtId { get; set; }

        public string UserId { get; set; }

        public bool Used { get; set; }

        public bool Invalidated { get; set; }

        public DateTime ExpireAt { get; set; }
    }
}