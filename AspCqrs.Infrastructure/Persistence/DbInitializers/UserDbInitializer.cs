using System.Collections.Generic;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Security;
using AspCqrs.Domain.Entities;
using AspCqrs.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AspCqrs.Infrastructure.Persistence.DbInitializers
{
    public class UserDbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogger<UserDbInitializer> _logger;

        public UserDbInitializer(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IDateTimeService dateTimeService,
            ILogger<UserDbInitializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dateTimeService = dateTimeService;
            _logger = logger;
        }
        
        public async Task Initialize()
        {
            const string userName = "SuperAdmin";
            const string userEmail = "super@cqrs.com";
            const string userPassword = "Super1234";
            var userRoles= new List<string>{ Role.SuperAdmin, Role.Admin };
            
            _logger.LogInformation($"Creating user '{userName}'...");

            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                _logger.LogWarning($"User '{userName}' already exists.");
                return;
            }

            var superAdminRole = await _roleManager.FindByNameAsync(Role.SuperAdmin);
            if (superAdminRole == null)
            {
                _logger.LogError($"Failed to create user '{userName}', Role '{Role.SuperAdmin}' not found.");
                return;
            }
            
            var adminRole = _roleManager.FindByNameAsync(Role.Admin);
            if (adminRole == null)
            {
                _logger.LogError($"Failed to create user '{userName}', Role '{Role.Admin}' not found.");
                return;
            }

            user = new ApplicationUser
            {
                UserName = userName,
                Email = userEmail,
                EmailConfirmed = true,
                User = new DomainUser
                {
                    UserName = userName,
                    Created = _dateTimeService.Now,
                }
            };
            var createResult = await _userManager.CreateAsync(user, userPassword);

            if (createResult.Succeeded)
            {
                _logger.LogInformation($"User '{userName}' created successfully.");
                
                var roleResult = await _userManager.AddToRolesAsync(user, userRoles);
                
                if (roleResult.Succeeded)
                    _logger.LogInformation($"Roles '{Role.SuperAdmin}' and '{Role.Admin}' assigned to user '{userName}' successfully.");
                else
                    _logger.LogError($"Failed to assign roles '{Role.SuperAdmin}' and '{Role.Admin}' to user '{userName}'");
            }
            else
                _logger.LogError($"Failed to create user '{userName}'.");
        }
    }
}