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
    /// <summary>
    /// Controller of products. User does not hae to be authorized.
    /// </summary>
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserOrdersService _ordersService;

        /// <summary>
        /// Creates ProductsController.
        /// </summary>
        /// <param name="context">Application DB Context</param>
        /// <param name="ordersService">user orders service</param>
        public ProductsController(AppDbContext context, UserOrdersService ordersService)
        {
            _context = context;
            _ordersService = ordersService;
        }

        /// <summary>
        /// Main page for viewing products and filterinf them.
        /// </summary>
        /// <param name="subCategoryId">Products only from specific SubCategory</param>
        /// <param name="priceFrom">Minimal price</param>
        /// <param name="priceTo">Maximal price</param>
        /// <param name="searchString">Product name has to contain this string.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Details of product with <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Product ID</param>
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

        /// <summary>
        /// API POST Request.
        /// Adds product with <paramref name="productId"/> into the user Cart.
        /// User needs to be authorized.
        /// </summary>
        /// <param name="productId">Product ID to add into the Cart</param>
        /// <returns>JSON object with <c>response.success</c> and new <c>response.prodQuantity</c>.</returns>
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

        /// <summary>
        /// API POST Request.
        /// Removes product with <paramref name="productId"/> from the user Cart.
        /// User needs to be authorized.
        /// </summary>
        /// <param name="productId">Product ID to remove from the Cart</param>
        /// <returns>JSON object with <c>response.success</c> and new <c>response.prodQuantity</c>.</returns>
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

        /// <summary>
        /// User needs to be authorized in "Admin" role.
        /// </summary>
        /// <returns>View for new Product creation.</returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["SubCategories"] = GetSubCategoriesSelectList();
            return View();
        }

        /// <summary>
        /// User needs to be authorized in "Admin" role.
        /// </summary>
        /// <param name="product">Product to create</param>
        /// <returns>View of newly created product.</returns>
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

        /// <summary>
        /// User needs to be authorized in "Admin" role.
        /// </summary>
        /// <param name="product">ID of product to edit</param>
        /// <returns>View for product settings.</returns>
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

        /// <summary>
        /// User needs to be authorized in "Admin" role.
        /// </summary>
        /// <param name="id">ID of product to edit</param>
        /// <param name="product">Changed product</param>
        /// <returns>Index if success. Else not.</returns>
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

        /// <summary>
        /// User needs to be authorized in "Admin" role.
        /// </summary>
        /// <param name="product">ID of product to delete</param>
        /// <returns>View for product deletion.</returns>
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

        /// <summary>
        /// User needs to be authorized in "Admin" role.
        /// </summary>
        /// <param name="id">ID of product to delete</param>
        /// <param name="product">Changed product</param>
        /// <returns>Index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            product.Deleted = true;
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
