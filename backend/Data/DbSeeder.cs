using backend.Models;

namespace backend.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        SeedRoles(context);
        SeedUsers(context);
    }

    private static void SeedRoles(AppDbContext context)
    {
        if (context.Roles.Any()) return;

        context.Roles.AddRange(
            new Role { Name = "public", Description = "ผู้ใช้งานทั่วไป" },
            new Role { Name = "staff", Description = "เจ้าหน้าที่" }
        );
        context.SaveChanges();
    }

    private static void SeedUsers(AppDbContext context)
    {
        if (context.Users.Any()) return;

        var publicRole = context.Roles.First(r => r.Name == "public");
        var staffRole = context.Roles.First(r => r.Name == "staff");

        context.Users.AddRange(
            new User
            {
                Email = "public@test.com",
                Password = BCrypt.Net.BCrypt.HashPassword("public123"),
                FullName = "Public User",
                RoleId = publicRole.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Email = "staff@test.com",
                Password = BCrypt.Net.BCrypt.HashPassword("staff123"),
                FullName = "Staff User",
                RoleId = staffRole.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
        context.SaveChanges();
    }
}
