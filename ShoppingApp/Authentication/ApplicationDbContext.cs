using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Models;
using ShoppingApp.Resource;
using System.Reflection.Emit;

namespace ShoppingApp.Authentication
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet <User> user { get; set; }

        public DbSet <UserRole> userRoles { get; set; } 

        public DbSet <Product> products { get; set; }

        public DbSet <Category> categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserRole>().HasData(
   new UserRole { RoleId = 1, RoleName = "User" },
   new UserRole { RoleId = 2, RoleName = "Admin" }
);
            builder.Entity<Category>().HasData(
  new Category { CategoryId = 1, CategoryName = "Clothing" },
  new Category { CategoryId = 2, CategoryName = "Electronic" },
  new Category { CategoryId = 3, CategoryName = "Cosmetics" }
  );

           base.OnModelCreating(builder);
        }
    }
}
