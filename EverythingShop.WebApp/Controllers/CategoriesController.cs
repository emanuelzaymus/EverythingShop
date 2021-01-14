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
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

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

        public async Task<IActionResult> MainCategoryDetails(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Index));
            }

            MainCategory mainCategory = await _context.MainCategories.Where(mc => mc.Id == id.Value).FirstOrDefaultAsync();
            return View(mainCategory);
        }

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

        public IActionResult MainCategoryCreate()
        {
            return View();
        }

        public IActionResult SubCategoryCreate()
        {
            ViewData["MainCategories"] = GetMainCategoriesSelectList();
            return View();
        }

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
