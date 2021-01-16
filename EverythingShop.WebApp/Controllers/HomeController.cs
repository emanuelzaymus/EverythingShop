using EverythingShop.WebApp.Data;
using EverythingShop.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;

namespace EverythingShop.WebApp.Controllers
{
    /// <summary>
    /// Controller for basic pages.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Creates HomeController.
        /// </summary>
        /// <param name="context">Application DB Context</param>
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Home page with 8 random products.
        /// </summary>
        public IActionResult Index()
        {
            var radnomProducts = _context.Products.Where(p => !p.Deleted).OrderBy(r => Guid.NewGuid()).Take(8).ToList();
            return View(radnomProducts);
        }

        /// <summary>
        /// Privacy page.
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// About Us page.
        /// </summary>
        public IActionResult AboutUs()
        {
            return View();
        }

        /// <summary>
        /// Error page.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
