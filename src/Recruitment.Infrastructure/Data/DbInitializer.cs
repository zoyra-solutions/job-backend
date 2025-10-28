using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Recruitment.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext db, ILogger? logger = null)
        {
            try
            {
                var pending = await db.Database.GetPendingMigrationsAsync();
                if (pending.Count > 0)
                {
                    await db.Database.MigrateAsync();
                }
                else
                {
                    // For fresh dev environments without migrations yet
                    await db.Database.EnsureCreatedAsync();
                }
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "Database migration failed; attempting EnsureCreated.");
                await db.Database.EnsureCreatedAsync();
            }

            // Basic seed: admin user and a default company
            if (!await db.Users.AnyAsync())
            {
                var now = DateTimeOffset.UtcNow;

                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = "admin@example.com",
                    Phone = "+84000000000",
                    FullName = "System Admin",
                    PasswordHash = "",
                    Role = "Admin",
                    CompanyId = null,
                    Avatar = null,
                    IsActive = true,
                    IsVerified = true,
                    TwoFactorEnabled = false,
                    LastLoginAt = null,
                    CreatedAt = now,
                    UpdatedAt = now,
                    BankAccountInfo = null,
                    BankAccountInfo_IV = null
                };

                await db.Users.AddAsync(adminUser);

                var defaultCompany = new Company
                {
                    Id = Guid.NewGuid(),
                    Name = "Default Company",
                    TaxCode = "",
                    Description = "Initial seeded company",
                    Logo = null,
                    Website = null,
                    Address = "",
                    Province = "",
                    District = "",
                    Ward = "",
                    BankInfo = null,
                    BankInfo_IV = null,
                    AdminUserId = adminUser.Id,
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                };

                await db.Companies.AddAsync(defaultCompany);
                await db.SaveChangesAsync();

                // Link admin to company (optional)
                adminUser.CompanyId = defaultCompany.Id;
                db.Users.Update(adminUser);
                await db.SaveChangesAsync();

                logger?.LogInformation("Seeded admin user and default company.");
            }
        }
    }
}