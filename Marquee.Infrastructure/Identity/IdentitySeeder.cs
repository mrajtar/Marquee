using Marquee.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Marquee.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        IServiceProvider services)
    {
        var roleManager =
            services.GetRequiredService<
                RoleManager<IdentityRole<int>>>();

        var userManager =
            services.GetRequiredService<
                UserManager<User>>();

        const string adminRole = "Admin";

        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            await roleManager.CreateAsync(
                new IdentityRole<int>(adminRole));
        }

        const string adminUsername = "admin";
        const string adminEmail = "admin@marquee.local";
        const string adminPassword = "Admin123!";

        var admin = await userManager.FindByNameAsync(
            adminUsername);

        if (admin is null)
        {
            admin = new User
            {
                UserName = adminUsername,
                Email = adminEmail,
                EmailConfirmed = true,
                DisplayName = "Marquee Admin",
                CreatedAt = DateTime.UtcNow
            };

            var createResult =
                await userManager.CreateAsync(
                    admin,
                    adminPassword);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    createResult.Errors.Select(
                        e => e.Description));

                throw new InvalidOperationException(
                    $"Failed to create admin user: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(
                admin,
                adminRole))
        {
            await userManager.AddToRoleAsync(
                admin,
                adminRole);
        }
    }
}