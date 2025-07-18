using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulseAPI.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Create Reader and Writer roles
            var readerRoleId = "1117baf9-d0a1-4eae-add2-277d3b6cf0a4";
            var writerRoleId = "3fffbe03-96d3-409b-9964-3ef777e311b0";

            var roles = new List<IdentityRole>
            {
                 new IdentityRole
                 {
                     Id= readerRoleId,
                     Name = "Reader",
                     NormalizedName="Reader".ToUpper(),
                     ConcurrencyStamp=readerRoleId
                 },
                 new IdentityRole
                 {
                     Id= writerRoleId,
                     Name="Writer",
                     NormalizedName="Writer".ToUpper(),
                     ConcurrencyStamp=writerRoleId
                 }
            };

            // Seed Roles
            builder.Entity<IdentityRole>().HasData(roles);

            // Create Admin user
            var adminUserId = "1ad3ceb9-ac5b-404b-8167-c5c633464923";

            var adminUser = new IdentityUser
            {
                Id = adminUserId,
                UserName = "admin@samplesite.com",
                Email = "admin@samplesite.com",
                NormalizedEmail = "admin@samplesite.com".ToUpper(),
                NormalizedUserName = "admin@samplesite.com".ToUpper()
            };

            adminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(adminUser, "admin@123");

            // Seed admin user
            builder.Entity<IdentityUser>().HasData(adminUser);

            // Roles for admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>
                {
                    UserId = adminUserId,
                    RoleId=readerRoleId
                },
                new IdentityUserRole<string>
                { 
                    UserId = adminUserId,
                    RoleId=writerRoleId}
            };

            // Seed roles for admin
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
