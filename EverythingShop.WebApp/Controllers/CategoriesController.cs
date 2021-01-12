using EverythingShop.WebApp.Data;
using EverythingShop.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
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
            var allCategories = await _context.MainCategories.Include(mc => mc.SubCategories).ToListAsync(); ;
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
                return new SelectList(_context.MainCategories, "Id", "Name", selectedCategoryId.Value);
            }
            return new SelectList(_context.MainCategories, "Id", "Name",
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
            return RedirectToAction(nameof(MainCategoryDetails), id);
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
            return RedirectToAction(nameof(SubCategoryDetails), id);
        }

        public async Task<IActionResult> MainCategoryDelete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Index));
            }
            MainCategory mainCategory = await _context.MainCategories.FindAsync(id.Value);
            return View("Delete", mainCategory);
        }

        public async Task<IActionResult> SubCategoryDelete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Index));
            }
            SubCategory subCategory = await _context.SubCategories.FindAsync(id.Value);
            return View("Delete", subCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            throw new NotImplementedException();
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
