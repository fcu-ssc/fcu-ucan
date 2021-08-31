using System;
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

            var recorder = new ApplicationRole
            {
                Name = "Recorder",
                NormalizedName = "Recorder".ToUpperInvariant()
            };
            await roleManager.CreateAsync(recorder);
            await roleManager.AddClaimAsync(recorder, new Claim(ClaimTypes.Role, "Recorder"));
            
            var member = new ApplicationRole
            {
                Name = "Member",
                NormalizedName = "Member".ToUpperInvariant()
            };
            await roleManager.CreateAsync(member);
            await roleManager.AddClaimAsync(member, new Claim(ClaimTypes.Role, "Member"));
            
            var user = new ApplicationRole
            {
                Name = "User",
                NormalizedName = "User".ToUpperInvariant()
            };
            await roleManager.CreateAsync(user);
            await roleManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "User"));
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
            await userManager.AddToRoleAsync(user, "Recorder");
            await userManager.AddToRoleAsync(user, "Member");
            await userManager.AddToRoleAsync(user, "User");
        }
    }
}