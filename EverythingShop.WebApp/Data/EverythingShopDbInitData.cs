using EverythingShop.WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EverythingShop.WebApp.Data
{
    public static class EverythingShopDbInitData
    {
        internal static void InitializeWithSampleData(IServiceProvider serviceProvider)
        {
            using (var context = new EverythingShopContext(
                serviceProvider.GetRequiredService<DbContextOptions<EverythingShopContext>>()))
            {
                //if (context.MainCategories.Any())
                //{
                //    return;
                //}

                // Remove all
                context.Products.RemoveRange(context.Products);
                context.SubCategories.RemoveRange(context.SubCategories);
                context.MainCategories.RemoveRange(context.MainCategories);
                context.SaveChanges();

                // Create all
                var electronics = new MainCategory() { Name = "Electronics" };
                var furniture = new MainCategory() { Name = "Furniture" };
                context.MainCategories.AddRange(electronics, furniture);

                var phones = new SubCategory() { MainCategory = electronics, Name = "Phones" };
                var kitchenAppl = new SubCategory() { MainCategory = electronics, Name = "Kitchen Appliences" };
                var desks = new SubCategory() { MainCategory = furniture, Name = "Desks" };
                context.SubCategories.AddRange(phones, kitchenAppl);

                context.Products.AddRange(
                    new Product() { SubCategory = phones, Name = "Samsung 5s", Price = 100 },
                    new Product() { SubCategory = phones, Name = "iPhone 6", Price = 15.99M },

                    new Product() { SubCategory = kitchenAppl, Name = "Microwave Oven", Price = 23 },

                    new Product() { SubCategory = desks, Name = "Wooden desk", Price = 10.20M }
                );

                context.SaveChanges();
            }
        }

    }
}
