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
using Microsoft.AspNetCore.Authorization;

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
            var products = _context.Products.Where(p => !p.Deleted).Include(p => p.SubCategory).Select(p => p);

            if (subCategoryId.HasValue)
                products = products.Where(p => p.SubCategoryId == subCategoryId.Value);

            if (priceFrom.HasValue)
                products = products.Where(p => p.Price >= priceFrom.Value);

            if (priceTo.HasValue)
                products = products.Where(p => p.Price <= priceTo.Value);

            if (!string.IsNullOrEmpty(searchString))
                products = products.Where(p => p.Name.Contains(searchString));

            List<MainCategory> allCategories = await _context.MainCategories.Include(m => m.SubCategories).AsNoTracking().ToListAsync();

            foreach (var mainCategory in allCategories)
                mainCategory.SubCategories.RemoveAll(sc => sc.Deleted);

            allCategories.RemoveAll(mc => mc.SubCategories.Count == 0);

            SearchProductViewModel ret = new SearchProductViewModel()
            {
                AllCategories = allCategories,
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

            if (!product.Deleted)
            {
                ViewData["QuantityOfProduct"] = User.Identity.IsAuthenticated
                    ? await _ordersService.GetQuantityOfProductInCart(User, id.Value) : 0;
            }

            return View(product);
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> AddProductToCart(int? productId)
        {
            int prodQuantity = -1;
            if (productId != null)
            {
                prodQuantity = await _ordersService.AddProductToCart(User, productId.Value);
            }
            return base.Json(new { newProductQuantity = prodQuantity, success = prodQuantity >= 0 });
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> RemoveProductFromCart(int? productId)
        {
            int prodQuantity = -1;
            if (productId != null)
            {
                prodQuantity = await _ordersService.RemoveProductFromCart(User, productId.Value);
            }
            return base.Json(new { newProductQuantity = prodQuantity, success = prodQuantity >= 0 });
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["SubCategories"] = GetSubCategoriesSelectList();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Name,Description,Picture,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.Deleted = false;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubCategories"] = GetSubCategoriesSelectList(product.SubCategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            if (product.Deleted)
            {
                return RedirectToAction(nameof(Details), new { id });
            }
            ViewData["SubCategories"] = GetSubCategoriesSelectList(product.SubCategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubCategories"] = GetSubCategoriesSelectList(product.SubCategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
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
            if (product.Deleted)
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            product.Deleted = true;
            //_context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private SelectList GetSubCategoriesSelectList(object selectedValue = null)
        {
            if (selectedValue != null)
            {
                return new SelectList(_context.SubCategories.Where(sc => !sc.Deleted).Include(sc => sc.MainCategory),
                "Id", "Name", selectedValue, "MainCategory");
            }
            return new SelectList(_context.SubCategories.Where(sc => !sc.Deleted).Include(sc => sc.MainCategory),
                "Id", "Name", _context.SubCategories.Select(sc => sc.Id).FirstOrDefault(), "MainCategory");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
