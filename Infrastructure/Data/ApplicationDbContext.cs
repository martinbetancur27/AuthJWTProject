using Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.ToTable("User");
                user.HasKey(p => p.Id);
                user.Property(p => p.Id).ValueGeneratedOnAdd();
                user.Property(p => p.Name).IsRequired();
                user.Property(p => p.UserName).IsRequired();
                user.Property(p => p.Password).IsRequired();
                user.HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity<UserRole>(
                    x => x.HasOne(p => p.User)
                    .WithMany().HasForeignKey(x => x.IdUser));
            });


            modelBuilder.Entity<Role>(role =>
            {
                role.ToTable("Role");
                role.HasKey(p => p.Id);
                role.Property(p => p.Id).ValueGeneratedOnAdd();
                role.Property(p => p.Name).IsRequired();
                role.Property(p => p.Description).IsRequired();
                role.HasMany(x => x.Users)
                .WithMany(x => x.Roles)
                .UsingEntity<UserRole>(
                x => x.HasOne(p => p.Role)
                    .WithMany().HasForeignKey(x => x.IdRole));
            });

            modelBuilder.Entity<UserRole>(urole =>
            {
                urole.HasKey(p => p.Id);
                urole.Property(p => p.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
