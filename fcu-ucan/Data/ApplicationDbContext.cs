using fcu_ucan.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace fcu_ucan.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        
        public override DbSet<ApplicationUser> Users { get; set; }

        public override DbSet<ApplicationUserClaim> UserClaims { get; set; }

        public override DbSet<ApplicationUserLogin> UserLogins { get; set; }

        public override DbSet<ApplicationUserToken> UserTokens { get; set; }
        
        public override DbSet<ApplicationUserRole> UserRoles { get; set; }

        public override DbSet<ApplicationRole> Roles { get; set; }

        public override DbSet<ApplicationRoleClaim> RoleClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<ApplicationUser>(b =>
            {
                b.HasMany(e => e.Claims)
                    .WithOne(uc => uc.User)
                    .HasForeignKey(uc => uc.UserId);
                b.HasMany(e => e.Logins)
                    .WithOne(ul => ul.User)
                    .HasForeignKey(ul => ul.UserId);
                b.HasMany(e => e.Tokens)
                    .WithOne(ut => ut.User)
                    .HasForeignKey(ut => ut.UserId);
                b.HasMany(e => e.UserRoles)
                    .WithOne(ur => ur.User)
                    .HasForeignKey(ur => ur.UserId);
            });
            
            builder.Entity<ApplicationRole>(b =>
            {
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId);
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId);
            });

            builder.Entity<ApplicationUserClaim>();
            
            builder.Entity<ApplicationUserLogin>(b =>
            {
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
            });
            
            builder.Entity<ApplicationUserToken>(b => 
            {
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            });

            builder.Entity<ApplicationUserRole>(b =>
            {
                b.HasKey(r => new { r.UserId, r.RoleId });
            });
            
            builder.Entity<ApplicationRole>();
            
            builder.Entity<ApplicationRoleClaim>();
        }
    }
}