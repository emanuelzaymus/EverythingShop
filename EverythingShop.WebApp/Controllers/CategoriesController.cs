using EverythingShop.WebApp.Data;
using EverythingShop.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Controllers
{
    /// <summary>
    /// Controller for Categories management. Accessible only for user in "Admin" role.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Creates CategoriesController.
        /// </summary>
        /// <param name="context">Application DB Context</param>
        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Shows all MainCategories and Subcategories.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            List<MainCategory> allCategories = await _context.MainCategories.Where(mc => !mc.Deleted)
                .Include(mc => mc.SubCategories)
                .AsNoTracking()
                .ToListAsync();

            foreach (var mainCategory in allCategories)
                mainCategory.SubCategories.RemoveAll(sc => sc.Deleted);

            return View(allCategories);
        }

        /// <summary>
        /// Detail for <see cref="MainCategory"/>.
        /// </summary>
        /// <param name="id"><see cref="MainCategory"/> ID.</param>
        public async Task<IActionResult> MainCategoryDetails(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Index));
            }

            MainCategory mainCategory = await _context.MainCategories.Where(mc => mc.Id == id.Value).FirstOrDefaultAsync();
            return View(mainCategory);
        }

        /// <summary>
        /// Detail for <see cref="SubCategory"/>.
        /// </summary>
        /// <param name="id"><see cref="SubCategory"/> ID.</param>
        public async Task<IActionResult> SubCategoryDetails(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Index));
            }

            SubCategory subCategory = await _context.SubCategories.Where(sc => sc.Id == id.Value).FirstOrDefaultAsync();
            ViewData["MainCategories"] = GetMainCategoriesSelectList(subCategory.MainCategoryId);
            return View(subCategory);
        }

        private SelectList GetMainCategoriesSelectList(int? selectedCategoryId = null)
        {

            if (selectedCategoryId.HasValue)
            {
                return new SelectList(_context.MainCategories.Where(mc => !mc.Deleted), "Id", "Name", selectedCategoryId.Value);
            }
            return new SelectList(_context.MainCategories.Where(mc => !mc.Deleted), "Id", "Name",
                _context.MainCategories.Select(mc => mc.Id).FirstOrDefault());
        }

        /// <summary>
        /// Page for <see cref="MainCategory"/> creation.
        /// </summary>
        /// <returns>View for <see cref="MainCategory"/> creation.</returns>
        public IActionResult MainCategoryCreate()
        {
            return View();
        }

        /// <summary>
        /// Page for <see cref="SubCategory"/> creation.
        /// </summary>
        /// <returns>View for <see cref="SubCategory"/> creation.</returns>
        public IActionResult SubCategoryCreate()
        {
            ViewData["MainCategories"] = GetMainCategoriesSelectList();
            return View();
        }

        /// <summary>
        /// Submit newly created MainCategory.
        /// </summary>
        /// <param name="mainCategory">Newly created MainCategory</param>
        /// <returns>Index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MainCategoryCreate([Bind("Name")] MainCategory mainCategory)
        {
            if (ModelState.IsValid)
            {
                mainCategory.Deleted = false;
                _context.Add(mainCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mainCategory);
        }

        /// <summary>
        /// Submit newly created SubCategory.
        /// </summary>
        /// <param name="subCategory">Newly created SubCategory</param>
        /// <returns>Index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubCategoryCreate([Bind("MainCategoryId,Name")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                subCategory.Deleted = false;
                _context.Add(subCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MainCategories"] = GetMainCategoriesSelectList();
            return View(subCategory);
        }

        /// <summary>
        /// Updates MainCategory with <paramref name="id"/>.
        /// </summary>
        /// <param name="id">MainCategory ID to update</param>
        /// <param name="mainCategory">Updated <see cref="MainCategory"/></param>
        /// <returns>Index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MainCategoryEdit(int id, [Bind("Id,Name")] MainCategory mainCategory)
        {
            if (id != mainCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mainCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.MainCategories.Any(mc => mc.Id == id))
                    {
                        return NotFound();
                    }
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(MainCategoryDetails), new { id });
        }

        /// <summary>
        /// Updates SubCategory with <paramref name="id"/>.
        /// </summary>
        /// <param name="id">SubCategory to update</param>
        /// <param name="mainCategory">Updated <see cref="SubCategory"/></param>
        /// <returns>Index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubCategoryEdit(int id, [Bind("Id,MainCategoryId,Name")] SubCategory subCategory)
        {
            if (id != subCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.SubCategories.Any(sc => sc.Id == id))
                    {
                        return NotFound();
                    }
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(SubCategoryDetails), new { id });
        }

        /// <summary>
        /// Delete MainCategory confirmation.
        /// </summary>
        /// <param name="id">MainCategory ID</param>
        /// <returns>Details</returns>
        public async Task<IActionResult> MainCategoryDelete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Index));
            }
            MainCategory mainCategory = await _context.MainCategories.FindAsync(id.Value);
            if (mainCategory.Deleted)
            {
                return RedirectToAction(nameof(MainCategoryDetails), new { id });
            }
            return View("Delete", mainCategory);
        }

        /// <summary>
        /// Delete SubCategory confirmation.
        /// </summary>
        /// <param name="id">SubCategory ID</param>
        /// <returns>Details</returns>
        public async Task<IActionResult> SubCategoryDelete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Index));
            }
            SubCategory subCategory = await _context.SubCategories.FindAsync(id.Value);
            if (subCategory.Deleted)
            {
                return RedirectToAction(nameof(SubCategoryDetails), new { id });
            }
            return View("Delete", subCategory);
        }

        /// <summary>
        /// Delete SubCategory confirmation.
        /// </summary>
        /// <param name="id">SubCategory ID</param>
        /// <returns>Details</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, bool? isMainCategory)
        {
            if (!id.HasValue || !isMainCategory.HasValue)
                return NotFound();

            if (isMainCategory.Value)
                await DeleteMainCategory(id);
            else
                await DeleteSubCategory(id);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task DeleteMainCategory(int? id)
        {
            if (!id.HasValue)
                return;

            MainCategory category = await _context.MainCategories.Where(mc => mc.Id == id && !mc.Deleted)
                .Include(mc => mc.SubCategories)
                .ThenInclude(sc => sc.Products)
                .FirstOrDefaultAsync();

            if (category != null)
            {
                category.Deleted = true;

                foreach (var subCategory in category.SubCategories)
                {
                    subCategory.Deleted = true;

                    foreach (var product in subCategory.Products)
                        product.Deleted = true;
                }
            }
        }

        private async Task DeleteSubCategory(int? id)
        {
            if (!id.HasValue)
                return;

            SubCategory category = await _context.SubCategories.Where(sc => sc.Id == id && !sc.Deleted)
                .Include(sc => sc.Products).FirstOrDefaultAsync();

            if (category != null)
            {
                category.Deleted = true;

                foreach (var product in category.Products)
                    product.Deleted = true;
            }
        }

    }
}
