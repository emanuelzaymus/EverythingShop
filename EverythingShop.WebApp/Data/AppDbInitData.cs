using EverythingShop.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;

namespace EverythingShop.WebApp.Data
{
    public static class AppDbInitData
    {
        internal static void Initialize(IServiceProvider services)
        {
            using (var context = new AppDbContext(services.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                context.Database.Migrate();

                InitializeWithSampleData(context);

                RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                CreateRoles(roleManager);
            }
        }

        private static void CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                Trace.WriteLine("CREATED !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }
        }

        private static void InitializeWithSampleData(AppDbContext context)
        {
            if (context.MainCategories.Any())
            {
                return;
            }

            // Remove all
            //context.Products.RemoveRange(context.Products);
            //context.SubCategories.RemoveRange(context.SubCategories);
            //context.MainCategories.RemoveRange(context.MainCategories);
            //context.SaveChanges();

            // Create all
            var electronics = new MainCategory() { Name = "Electronics" };
            var furniture = new MainCategory() { Name = "Furniture" };
            var toys = new MainCategory() { Name = "Toys" };
            context.MainCategories.AddRange(electronics, furniture, toys);

            var phones = new SubCategory() { MainCategory = electronics, Name = "Phones" };
            var kitchenAppl = new SubCategory() { MainCategory = electronics, Name = "Kitchen Appliences" };
            var laptops = new SubCategory() { MainCategory = electronics, Name = "Laptops" };
            var desks = new SubCategory() { MainCategory = furniture, Name = "Desks" };
            var lego = new SubCategory() { MainCategory = toys, Name = "Lego" };
            context.SubCategories.AddRange(phones, kitchenAppl, laptops, desks, lego);

            context.Products.AddRange(
                new Product() { SubCategory = phones, Name = "Samsung 5s", Price = 100, Description = "Possible procured her trifling laughter thoughts property she met way. Companions shy had solicitude favourable own. Which could saw guest man now heard but. Lasted my coming uneasy marked so should. Gravity letters it amongst herself dearest an windows by. Wooded ladies she basket season age her uneasy saw. Discourse unwilling am no described dejection incommode no listening of. Before nature his parish boy.", Picture = "https://drop.ndtv.com/TECH/product_database/images/2252014124325AM_635_samsung_galaxy_s5.jpeg" },
                new Product() { SubCategory = phones, Name = "iPhone 6", Price = 15.99M, Description = "Folly words widow one downs few age every seven. If miss part by fact he park just shew. Discovered had get considered projection who favourable. Necessary up knowledge it tolerably. Unwilling departure education is be dashwoods or an. Use off agreeable law unwilling sir deficient curiosity instantly. Easy mind life fact with see has bore ten. Parish any chatty can elinor direct for former. Up as meant widow equal an share least. ", Picture = "https://drop.ndtv.com/TECH/product_database/images/910201410645AM_635_apple_iphone_6_plus.jpeg" },
                new Product() { SubCategory = phones, Name = "Samsung Galaxy A20e", Price = 139.90M, Description = "So, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon, stop that pigeon. Howwww! Nab him, jab him, tab him, grab him, stop that pigeon now.", Picture = "https://cdn.alza.sk/ImgW.ashx?fd=f3&cd=SAMO0172b1" },
                new Product() { SubCategory = phones, Name = "iPhone SE 64GB", Price = 474.39M, Description = "Ulysses, Ulysses — Soaring through all the galaxies. In search of Earth, flying in to the night. Ulysses, Ulysses — Fighting evil and tyranny, with all his power, and with all of his might.", Picture = "https://cdn.alza.sk/Foto/FotoAdd/RI/RI031b2-01.jpg" },
                new Product() { SubCategory = phones, Name = "Samsung Galaxy S10e Dual", Price = 0, Description = "Thundercats are on the move, Thundercats are loose. Feel the magic, hear the roar, Thundercats are loose. Thunder, thunder, thunder, Thundercats! ", Picture = "https://cdn.alza.sk/ImgW.ashx?fd=f3&cd=SAMO0167b2" },
                new Product() { SubCategory = phones, Name = "HUAWEI P30 Lite", Price = 240, Description = "80 days around the world, we’ll find a pot of gold just sitting where the rainbow’s ending. Time — we’ll fight against the time, and we’ll fly on the white wings of the wind.", Picture = "https://cdn.alza.sk/ImgW.ashx?fd=f3&cd=HU3135b1" },
                new Product() { SubCategory = phones, Name = "Xiaomi Redmi Note 9 Pro LTE 64 GB", Price = 220, Description = "days around the world, no we won’t say a word before the ship is really back. Round, round, all around the world. Round, all around the world. Round, all around the world. Round, all around the world. ", Picture = "https://cdn.alza.sk/ImgW.ashx?fd=f3&cd=SKXI210b2" },

                new Product() { SubCategory = kitchenAppl, Name = "Microwave Oven", Price = 23, Description = "Another journey chamber way yet females man. Way extensive and dejection get delivered deficient sincerity gentleman age. Too end instrument possession contrasted motionless. Calling offence six joy feeling. Coming merits and was talent enough far. Sir joy northward sportsmen education. Discovery incommode earnestly no he commanded if. Put still any about manor heard. ", Picture = "https://images.samsung.com/is/image/samsung/africa-en-microwave-oven-grill-mg28j5255gs-mg28j5255gs-sm-frontsilver-165243468?$720_576_PNG$" },
                new Product() { SubCategory = kitchenAppl, Name = "Toaster", Price = 23, Description = "Village did removed enjoyed explain nor ham saw calling talking. Securing as informed declared or margaret. Joy horrible moreover man feelings own shy. Request norland neither mistake for yet. Between the for morning assured country believe. On even feet time have an no at", Picture = "https://target.scene7.com/is/image/Target/GUEST_d8a811c3-5f0a-4131-8abf-8dfca482136f?wid=488&hei=488&fmt=pjpeg" },
                new Product() { SubCategory = kitchenAppl, Name = "CATLER VB 8010", Price = 0.15M, Description = "Mutley, you snickering, floppy eared hound. When courage is needed, you’re never around. Those medals you wear on your moth-eaten chest should be there for bungling at which you are best.", Picture = "https://cdn.alza.sk/ImgW.ashx?fd=f3&cd=CATVB559" },

                new Product() { SubCategory = laptops, Name = "Lenovo V15-IIL Iron Grey", Price = 700.42M, Description = "The European languages are members of the same family. Their separate existence is a myth. For science, music, sport, etc, Europe uses the same vocabulary. The languages only differ in their grammar, their pronunciation and their most common words. Everyone realizes why a new common language would be desirable: one", Picture = "https://cdn.alza.sk/ImgW.ashx?fd=f3&cd=NT220b2d3e1" },
                new Product() { SubCategory = laptops, Name = "Macbook Pro 13", Price = 100000, Description = "80 days around the world, we’ll find a pot of gold just sitting where the rainbow’s ending. Time — we’ll fight against the time, and we’ll fly on the white wings of the wind. 80 days around the world, no we won’t say a word before the ship is really back. Round, round, all around the world. Round, all around the world. Round, all around the world. Round, all around the world.", Picture = "https://cdn.alza.sk/Foto/f3/NL/NL257a2a4.jpg" },
                new Product() { SubCategory = laptops, Name = "Dell Latitude 3510", Price = 750.33M, Description = "Top Cat! The most effectual Top Cat! Who’s intellectual close friends get to call him T.C., providing it’s with dignity. Top Cat! The indisputable leader of the gang. He’s the boss, he’s a pip, he’s the championship. He’s the most tip top, Top Cat.0", Picture = "https://cdn.alza.sk/ImgW.ashx?fd=f4&cd=ADC453f33l&i=1.jpg" },
                new Product() { SubCategory = laptops, Name = "MSI GL75 Leopard", Price = 1587.53M, Description = "Barnaby The Bear’s my name, never call me Jack or James, I will sing my way to fame, Barnaby the Bear’s my name. Birds taught me to sing, when they took me to their king, first I had to fly, in the sky so high so high, so high so high so high", Picture = "https://cdn.alza.sk/ImgW.ashx?fd=f3&cd=NB120a2p1k" },

                new Product() { SubCategory = desks, Name = "Wooden desk", Price = 10.20M, Description = "Between the for morning assured country believe. On even feet time have an no at. Relation so in confined smallest children unpacked delicate. Why sir end believe uncivil respect. Always get adieus nature day course for common. My little garret repair to desire he esteem. ", Picture = "https://tcdn.storeden.com/product/19379379/21269545" }
            );

            context.SaveChanges();
        }

    }
}
