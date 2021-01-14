using EverythingShop.WebApp.Data;
using EverythingShop.WebApp.Models;
using EverythingShop.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Controllers
{
    [Authorize]
    public class UserOrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserOrdersService _ordersService;

        public UserOrdersController(AppDbContext context, UserOrdersService ordersService)
        {
            _context = context;
            _ordersService = ordersService;
        }

        public async Task<IActionResult> Index()
        {
            List<UserOrder> orders = await _ordersService.GetFinishedOrders(User);
            return View(orders);
        }

        public async Task<IActionResult> Cart()
        {
            UserOrder cartContent = (await _ordersService.GetCurrentOrNewOrder(User));
            return View(cartContent);
        }

        public async Task<IActionResult> CompleteOrder()
        {
            UserOrder order = await _ordersService.GetCurrentOrderWithProducts(User);
            if (order == null || order.OrderProducts.Count == 0)
            {
                return NotFound();
            }
            return View(order);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteOrder([Bind("Id,UserId,ContactName,StreetAddress,PostalCode,City,Country")] UserOrder order)
        {
            if (!await _ordersService.IsCurrentOrderAsync(User, order) || await _ordersService.IsCurrentOrderEmpty(User))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _ordersService.CompleteOrder(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if ((await _ordersService.GetCurrentOrderWithoutProducts(User)) == null)
                    {
                        return NotFound();
                    }
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        [HttpPost]
        public async Task<JsonResult> SetOrderDelivered(int? orderId)
        {
            if (orderId.HasValue)
            {
                return Json(new { newOrderState = await _ordersService.SetOrderDelivered(User, orderId.Value) });
            }
            return Json(new { newOrderState = (string)null });
        }

    }
}
