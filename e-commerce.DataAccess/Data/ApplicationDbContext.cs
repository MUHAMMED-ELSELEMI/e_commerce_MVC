using ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "scifi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "history", DisplayOrder = 3 }

                );

            modelBuilder.Entity<Product>().HasData(
               new Product
               {
                   Id = 1,
                   ISBN = "Sdf34lsn5f",
                   Author = "Muhammed",
                   Description = "SCifi book",
                   ListPrice = 123.34,
                   Price = 200,
                   Price100 = 122,
                   Price50 = 180,
                   Title = "dark Knight",
                   CategoryId = 1,
                   ImageUrl = "http/:google"

               }
               );
        }

    }
}
