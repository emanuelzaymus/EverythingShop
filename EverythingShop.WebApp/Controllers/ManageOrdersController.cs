using EverythingShop.WebApp.Data;
using EverythingShop.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageOrdersController : Controller
    {
        private readonly AppDbContext _context;

        public ManageOrdersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<UserOrder> orders = await _context.UserOrders.Where(o => o.IsFinished() && o.State != OrderState.Delivered)
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .AsNoTracking().ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> DeliveredOrders()
        {
            List<UserOrder> orders = await _context.UserOrders.Where(o => o.IsFinished() && o.State == OrderState.Delivered)
               .Include(o => o.User)
               .Include(o => o.OrderProducts)
               .ThenInclude(op => op.Product)
               .AsNoTracking().ToListAsync();
            return View(orders);
        }

        [HttpPost]
        public async Task<string> SetOrderSent(int? orderId)
        {
            if (orderId.HasValue)
            {
                UserOrder userOrder = await _context.UserOrders.FindAsync(orderId);

                if (userOrder.State == OrderState.Pending)
                {
                    userOrder.State = OrderState.Sent;
                }
                await _context.SaveChangesAsync();
                return OrderState.Sent.ToString();
            }
            return null;
        }

    }
}
