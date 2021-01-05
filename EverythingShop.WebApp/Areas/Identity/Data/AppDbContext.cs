using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EverythingShop.WebApp.Areas.Identity.Data;
using EverythingShop.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EverythingShop.WebApp.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<MainCategory> MainCategories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<UserOrder> UserOrders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderProduct>().HasKey(nameof(OrderProduct.UserOrderId), nameof(OrderProduct.ProductId));

            base.OnModelCreating(builder);
        }
    }
}
