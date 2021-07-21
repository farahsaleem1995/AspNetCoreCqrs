using System.Collections.Generic;
using System.Linq;
using AspCqrs.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace AspCqrs.Infrastructure.Identity
{
    public static class SignInResultExtensions
    {
        public static Result ToApplicationResult(this SignInResult signInResult)
        {
            return signInResult.Succeeded
                ? Result.Success()
                : Result.BadRequest(new Dictionary<string, string[]>
                {
                    {"Sign In", new[] {"User name and password does not match"}}
                });
        }

        public static Result<TData> ToApplicationResult<TData>(this SignInResult signInResult)
        {
            return signInResult.Succeeded
                ? Result.Success<TData>(default)
                : Result.Unauthorized<TData>(new Dictionary<string, string[]>
                {
                    {"Sign In", new[] {"User name and password does not match"}}
                });
        }

        public static Result<TData> ToApplicationResult<TData>(this SignInResult signInResult, TData data)
        {
            return signInResult.Succeeded
                ? Result.Success(data)
                : Result.Unauthorized<TData>(new Dictionary<string, string[]>
                {
                    {"Sign In", new[] {"User name and password does not match"}}
                });
        }
    }
}