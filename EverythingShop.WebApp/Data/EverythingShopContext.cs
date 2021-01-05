using EverythingShop.WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Data
{
    public class EverythingShopContext : DbContext
    {
        public DbSet<MainCategory> MainCategories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<UserOrder> CustomerOrders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        public EverythingShopContext(DbContextOptions<EverythingShopContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>().HasKey(nameof(OrderProduct.UserOrderId), nameof(OrderProduct.ProductId));

            base.OnModelCreating(modelBuilder);
        }
    }
}
