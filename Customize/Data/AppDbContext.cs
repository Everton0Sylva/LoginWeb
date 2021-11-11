using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Customize.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<AuthClaim> AuthClaim { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*
            modelBuilder.Entity<AuthUser>(b =>
            {
                b.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();
                */

            modelBuilder.Entity<AuthUser>()
                .Ignore(p => p.Email)
                .Ignore(p => p.EmailConfirmed)
                .Ignore(p => p.SecurityStamp)
                .Ignore(p => p.PhoneNumber)
                .Ignore(p => p.PhoneNumberConfirmed)
                .Ignore(p => p.EmailConfirmed)
                .Ignore(p => p.TwoFactorEnabled);


            modelBuilder.Entity<AuthRoleClaim>()
                .HasMany(e => e.Claim)
                .WithOne()
                .HasForeignKey(uc => uc.ClaimId)
                .IsRequired()
                .Ignore(p => p.ClaimType)
                .Ignore(p => p.ClaimValue);


            modelBuilder.Entity<AuthUserClaim>()
                .Ignore(p => p.ClaimType)
                .Ignore(p => p.ClaimValue);

        }
    }
}