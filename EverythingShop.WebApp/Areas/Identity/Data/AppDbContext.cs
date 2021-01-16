using EverythingShop.WebApp.Areas.Identity.Data;
using EverythingShop.WebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EverythingShop.WebApp.Data
{
    /// <summary>
    /// Identity Application DB Context.
    /// </summary>
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        /// <summary>
        /// DB table MainCategories.
        /// </summary>
        public DbSet<MainCategory> MainCategories { get; set; }

        /// <summary>
        /// DB table SubCategories.
        /// </summary>
        public DbSet<SubCategory> SubCategories { get; set; }

        /// <summary>
        /// DB table Products.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// DB table UserOrders.
        /// </summary>
        public DbSet<UserOrder> UserOrders { get; set; }

        /// <summary>
        /// DB tables OrderProducts.
        /// </summary>
        public DbSet<OrderProduct> OrderProducts { get; set; }

        /// <summary>
        /// Creates AppDbContext.
        /// </summary>
        /// <param name="options">Database options</param>
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
