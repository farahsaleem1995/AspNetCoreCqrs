using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Models;
using AspCqrs.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspCqrs.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;

        public IdentityService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<Result<(string userId, IEnumerable<string> roles)>> CreateUserAsync(string userName,
            string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
                EmailConfirmed = true,
                User = new DomainUser
                {
                    UserName = userName,
                    Created = DateTime.UtcNow
                }
            };

            var result = await _userManager.CreateAsync(user, password);

            return result.ToApplicationResult((user.Id, new List<string>().AsEnumerable()));
        }

        public async Task<Result<(string userId, IEnumerable<string> roles)>> SignInAsync(string userName,
            string password)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            
            if (user == null)
                return Result.Unauthorized<(string userId, IEnumerable<string> roles)>(new Dictionary<string, string[]>
                {
                    {"Sign In", new[] {"User name and password does not match"}}
                });

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            var userRoles = await _userManager.GetRolesAsync(user);

            if (result.Succeeded)
            {
                return Result.Success<(string userId, IEnumerable<string> roles)>((user.Id, userRoles));
            }

            return result.ToApplicationResult<(string userId, IEnumerable<string> roles)>();
        }

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                return result.ToApplicationResult();
            }

            return Result.Success();
        }
    }
}