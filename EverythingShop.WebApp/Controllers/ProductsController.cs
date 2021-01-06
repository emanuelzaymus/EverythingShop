﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EverythingShop.WebApp.Data;
using EverythingShop.WebApp.Models;

namespace EverythingShop.WebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? mainCategory, int? subCategory, string searchString)
        {
            var products = _context.Products.Include(p => p.SubCategory).Select(p => p);

            if (mainCategory != null)
                products = products.Where(p => p.SubCategory.MainCategoryId == mainCategory);

            if (subCategory != null)
                products = products.Where(p => p.SubCategoryId == subCategory);

            if (!string.IsNullOrEmpty(searchString))
                products = products.Where(p => p.Name.Contains(searchString));

            SearchProductViewModel ret = new SearchProductViewModel()
            {
                AllCategories = await _context.MainCategories.Include(m => m.SubCategories).ToListAsync(),
                MainCategories = new SelectList(await _context.MainCategories.Include(m => m.SubCategories).ToListAsync(), nameof(MainCategory.Id), nameof(MainCategory.Name)),
                SubCategories = new SelectList(await _context.SubCategories.ToListAsync(), nameof(SubCategory.Id), nameof(SubCategory.Name)),
                Products = await products.ToListAsync(),
                MainCategory = mainCategory,
                SubCategory = subCategory,
                SearchString = searchString
            };

            return View(ret);
        }

        public async Task<IActionResult> MyMethod(string subCat)
        {
            int subCategory = -1;
            int.TryParse(subCat, out subCategory);

            var products = _context.Products.Where(p => p.SubCategoryId == subCategory).Include(p => p.SubCategory).Select(p => p);

            SearchProductViewModel ret = new SearchProductViewModel()
            {
                AllCategories = await _context.MainCategories.Include(m => m.SubCategories).ToListAsync(),
                MainCategories = new SelectList(await _context.MainCategories.Include(m => m.SubCategories).ToListAsync(), nameof(MainCategory.Id), nameof(MainCategory.Name)),
                SubCategories = new SelectList(await _context.SubCategories.ToListAsync(), nameof(SubCategory.Id), nameof(SubCategory.Name)),
                Products = await products.ToListAsync(),
                MainCategory = null,
                SubCategory = subCategory,
                SearchString = null
            };

            return View(ret);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
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
