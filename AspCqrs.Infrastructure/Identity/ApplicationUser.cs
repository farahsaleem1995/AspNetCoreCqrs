using System;
using AspCqrs.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AspCqrs.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public DomainUser User { get; set; }
    }
}