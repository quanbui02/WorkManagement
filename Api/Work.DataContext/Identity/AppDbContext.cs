using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Work.DataContext;
using Work.DataContext.Identity;

namespace Work.DataContext
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<AppPermission> AppPermission { get; set; }
        public DbSet<AppController> AppController { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<AppGrantedPermissions> AppGrantedPermissions { get; set; }
    }
}
