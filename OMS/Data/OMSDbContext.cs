using Microsoft.EntityFrameworkCore;
using OMS.Data.Models;

namespace OMS.Data
{
    public class OMSDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Shopper> Shoppers { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your connection string here
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=OMS;Trusted_Connection=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Basket>()
                .HasOne(b => b.Shopper)
                .WithMany(s => s.Baskets)
                .HasForeignKey(b => b.IdShopper)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BasketItem>()
                .HasOne(bi => bi.Basket)
                .WithMany(b => b.BasketItems)
                .HasForeignKey(bi => bi.IdBasket)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BasketItem>()
                .HasOne(bi => bi.Product)
                .WithMany(p => p.BasketItems)
                .HasForeignKey(bi => bi.IdProduct)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}