using CRUDWebAPI.Models.Authentication.SignUp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRUDWebAPI.Models
{
    public class RegisterUserDBContext : IdentityDbContext<IdentityUser>
    {
        public RegisterUserDBContext(DbContextOptions<RegisterUserDBContext> options) : base (options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        private void SeedRoles (ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                    new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                    new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User"}
                );
        }
    }
}
