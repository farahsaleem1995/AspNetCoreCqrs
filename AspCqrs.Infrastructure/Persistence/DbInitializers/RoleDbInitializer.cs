using System;
using System.Linq;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Security;
using AspCqrs.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AspCqrs.Infrastructure.Persistence.DbInitializers
{
    public class RoleDbInitializer : IDbInitializer
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<RoleDbInitializer> _logger;

        public RoleDbInitializer(RoleManager<ApplicationRole> roleManager,
            ILogger<RoleDbInitializer> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task Initialize()
        {
            var roleFields = typeof(Role).GetFields().Where(info => info.FieldType == typeof(string));

            foreach (var fieldInfo in roleFields)
            {
                var roleName = fieldInfo.GetValue(null)?.ToString();
                
                if (string.IsNullOrEmpty(roleName)) continue;
                
                _logger.LogInformation($"Creating role '{roleName}'...");

                var isRoleExist = await _roleManager.RoleExistsAsync(roleName);
                if (isRoleExist)
                {
                    _logger.LogWarning($"Role '{roleName}' already exists.");
                    continue;
                }

                var result = await _roleManager.CreateAsync(new ApplicationRole
                {
                    Name = roleName
                });

                if (result.Succeeded)
                    _logger.LogInformation($"Role '{roleName}' created successfully.");
                else
                    _logger.LogError($"Failed to create role '{roleName}'.");
            }
        }
    }
}