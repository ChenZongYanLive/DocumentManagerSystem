using DocumentLibrary.MySql.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace DocumentLibrary.MySql
{
    public class AccountDbContext : IdentityDbContext<Account, Role, Guid>
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Account>().ToTable("Account").HasIndex(c => new { c.NormalizedUserName }).IsUnique();
            builder.Entity<Role>().ToTable("Role");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("AccountRole");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AccountClaim");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AccountLogin");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("AccountToken");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaim");
        }
    }
}
