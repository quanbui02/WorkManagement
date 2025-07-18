using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Work.DataContext;
using Work.DataContext.Models;
using WorkManagement.Models;

public static class SeedHelper
{
    public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var WmContext = scope.ServiceProvider.GetRequiredService<WorkManagementContext>();

        var adminUserName = "admin";
        var adminEmail = "admin@wmm.com";
        var adminPassword = "Admin@123";
        var adminRoleName = "Admin";

        if (!await roleManager.RoleExistsAsync(adminRoleName))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRoleName));
        }

        var adminUser = await userManager.FindByNameAsync(adminUserName);
        if (adminUser == null)
        {
            adminUser = new AppUser
            {
                UserName = adminUserName,
                Email = adminEmail,
                EmailConfirmed = true,
                IsSuperUser = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, adminRoleName);
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Không thể tạo admin: {errors}");
            }
        }

        var existingBusinessUser = await WmContext.Users
            .FirstOrDefaultAsync(x => x.UserIdGuid == adminUser.Id);

        if (existingBusinessUser == null)
        {
            WmContext.Users.Add(new Users
            {
                UserIdGuid = adminUser.Id,
                UserName = adminUser.UserName,
                Name = "Admin",
                Email = adminUser.Email,
                Phone = "0941254910",
                IsApproveCmt = true,
                IsApproved = true,
                DomainStatus = 1,
                IdType = 1,
                Balance = 0,
                TotalReward = 0,
                CashInTransit = 0
            });

            await WmContext.SaveChangesAsync();
        }
    }

}

