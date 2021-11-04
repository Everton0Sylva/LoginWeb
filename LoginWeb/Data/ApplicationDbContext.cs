using LoginWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project2.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoginWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AuthToken> AuthToken { get; set; }
        public DbSet<AuthClaim> AuthClaim { get; set; }
        public DbSet<RoleClaim> RoleClaim { get; set; }
        public DbSet<UserClaim> UserClaim { get; set; }
    }
}
