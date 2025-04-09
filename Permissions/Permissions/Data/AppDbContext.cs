using Microsoft.EntityFrameworkCore;
using Permissions.Models;

namespace Permissions.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>()
                .HasOne(p => p.PermissionType)
                .WithMany()
                .HasForeignKey(p => p.PermissionTypeId); 

            modelBuilder.Entity<PermissionType>().HasData(
                new PermissionType { Id = 1, Description = "Read" },
                new PermissionType { Id = 2, Description = "Write" },
                new PermissionType { Id = 3, Description = "Execute" }
            );
        }
    }
}