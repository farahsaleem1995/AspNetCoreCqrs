using System.Collections.Generic;
using AspCqrs.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace AspCqrs.Infrastructure.Identity
{
    public static class SignInResultExtensions
    {
        public static Result ToApplicationResult(this SignInResult result)
        {
            var errors = new Dictionary<string, string[]>();

            if (result.IsLockedOut)
                errors.Add("LockedOut", new[] {"User is locked out currently."});

            if (result.IsNotAllowed)
                errors.Add("NotAllowed", new[] {"User is not allowed to login."});

            if (result.RequiresTwoFactor)
                errors.Add("RequiresTwoFactor", new[] {"Two factor authentication is required."});

            return result.Succeeded
                ? Result.Success()
                : Result.Failure(errors);
        }
    }
}