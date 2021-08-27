using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using fcu_ucan.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace fcu_ucan.Data
{
    public class DataSeeder
    {
        public static async Task Initialize(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<DataSeeder>>();
                var dbContext = services.GetRequiredService<ApplicationDbContext>();
                
                logger.LogInformation("開始創建資料庫");
                if (await dbContext.Database.EnsureCreatedAsync())
                {
                    logger.LogInformation("開始創建角色");
                    await CreateRoleAsync(services, logger);
                    logger.LogInformation("創建角色完成");
                    
                    logger.LogInformation("開始創建使用者");
                    await CreateUserAsync(services, logger);
                    logger.LogInformation("創建角色完成");
                }
                else
                {
                    logger.LogInformation("資料庫已存在");
                }
            }
        }

        private static async Task CreateRoleAsync(IServiceProvider services, ILogger<DataSeeder> logger)
        {
            var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
            var role = new ApplicationRole
            {
                Name = "Administrator",
                NormalizedName = "Administrator".ToUpperInvariant()
            };
            await roleManager.CreateAsync(role);
            await roleManager.AddClaimAsync(role, new Claim(ClaimTypes.Role, "Administrator"));
        }

        private static async Task CreateUserAsync(IServiceProvider services, ILogger<DataSeeder> logger)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var user = new ApplicationUser
            {
                UserName = "Admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                IsEnable = true
            };
            await userManager.CreateAsync(user, "Admin1234");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.SecurityStamp)
            };
            await userManager.AddClaimsAsync(user, claims);
            await userManager.AddToRoleAsync(user, "Administrator");
        }
    }
}