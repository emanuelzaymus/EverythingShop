﻿using EverythingShop.WebApp.Data;
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
            List<OrderProduct> cartContent = await _ordersService.GetCartContent(User);
            return View(cartContent);
        }

        public async Task<IActionResult> CompleteOrder()
        {
            UserOrder order = await _ordersService.GetCurrentOrder(User);
            if (order == null)
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
            if (!await _ordersService.IsCurrentOrderAsync(order, User))
                return NotFound();

            order.OrderedOn = DateTime.Now;
            order.State = OrderState.Pending;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if ((await _ordersService.GetCurrentOrder(User)) == null)
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
