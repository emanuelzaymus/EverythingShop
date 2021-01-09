using EverythingShop.WebApp.Data;
using EverythingShop.WebApp.Models;
using EverythingShop.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Controllers
{
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
            // TODO: if the order is empty return NotFound();
            if (!await _ordersService.IsCurrentOrderAsync(User, order))
                return NotFound();

            order.OrderedOn = DateTime.Now; // del
            order.State = OrderState.Pending; // del

            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: remoake: _orderService.Order(order);
                    _context.Update(order); // del
                    await _context.SaveChangesAsync(); // del
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

    }
}
