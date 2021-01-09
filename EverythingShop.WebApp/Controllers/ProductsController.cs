using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EverythingShop.WebApp.Data;
using EverythingShop.WebApp.Models;
using EverythingShop.WebApp.Services;

namespace EverythingShop.WebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserOrdersService _ordersService;

        public ProductsController(AppDbContext context, UserOrdersService ordersService)
        {
            _context = context;
            _ordersService = ordersService;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? subCategoryId, int? priceFrom, int? priceTo, string searchString)
        {
            var products = _context.Products.Include(p => p.SubCategory).Select(p => p);

            if (subCategoryId.HasValue)
                products = products.Where(p => p.SubCategoryId == subCategoryId.Value);

            if (priceFrom.HasValue)
                products = products.Where(p => p.Price >= priceFrom.Value);

            if (priceTo.HasValue)
                products = products.Where(p => p.Price <= priceTo.Value);

            if (!string.IsNullOrEmpty(searchString))
                products = products.Where(p => p.Name.Contains(searchString));

            SearchProductViewModel ret = new SearchProductViewModel()
            {
                AllCategories = await _context.MainCategories.Include(m => m.SubCategories).ToListAsync(),
                Products = await products.ToListAsync(),
                SubCategoryId = subCategoryId
            };
            return View(ret);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
                return NotFound();

            ViewData["QuantityOfProduct"] = await _ordersService.GetQuantityOfProductInCart(User, id.Value);
            return View(product);
        }

        [HttpPost]
        public async Task<int> AddProductToCart(int? productId)
        {
            if (productId == null)
                return -1;

            return await _ordersService.AddProductToCart(User, productId.Value);
        }

        [HttpPost]
        public async Task<int> RemoveProductFromCart(int? productId)
        {
            if (productId == null)
                return -1;

            return await _ordersService.RemoveProductFromCart(User, productId.Value);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Name,Description,Picture,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "Name", product.SubCategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "Name", product.SubCategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,Name,Description,Picture,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "Name", product.SubCategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
