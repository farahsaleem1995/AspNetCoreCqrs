using System.Linq;
using AspCqrs.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace AspCqrs.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult identityResult)
        {
            return identityResult.Succeeded
                ? Result.Success()
                : Result.Failure(identityResult.Errors.Select(e => e.Description));
        }
        
        public static Result<TData> ToApplicationResult<TData>(this IdentityResult identityResult)
        {
            return identityResult.Succeeded
                ? Result.Success<TData>(default)
                : Result.Failure<TData>(identityResult.Errors.Select(e => e.Description));
        }
        
        public static Result<TData> ToApplicationResult<TData>(this IdentityResult identityResult, TData data)
        {
            return identityResult.Succeeded
                ? Result.Success(data)
                : Result.Failure<TData>(identityResult.Errors.Select(e => e.Description));
        }
    }
}