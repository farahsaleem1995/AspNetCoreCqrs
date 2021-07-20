using System.Collections.Generic;
using AspCqrs.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace AspCqrs.Infrastructure.Identity
{
    public static class SignInResultExtensions
    {
        public static Result ToApplicationResult(this SignInResult signInResult)
        {
            return signInResult.Succeeded ? Result.Success() : Result.Failure(new List<string>{ "User name and password does not match"});
        }
    }
}