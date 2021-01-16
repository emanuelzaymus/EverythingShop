using EverythingShop.WebApp.Data;
using EverythingShop.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Controllers
{
    /// <summary>
    /// Controller for Order management. Accessible only for user in "Admin" role.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class ManageOrdersController : Controller
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Creates ManageOrdersController.
        /// </summary>
        /// <param name="context">Application DB Context</param>
        public ManageOrdersController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Shows all not delivered UserOrders.
        /// </summary>
        /// <returns>Returns View for all not delivered UserOrders.</returns>
        public async Task<IActionResult> Index()
        {
            List<UserOrder> orders = await _context.UserOrders.Where(o => o.State.HasValue && o.State != OrderState.Delivered)
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .AsNoTracking().ToListAsync();
            return View(orders);
        }

        /// <summary>
        /// Shows all delivered UserOrders.
        /// </summary>
        /// <returns>Returns View for all delivered UserOrders.</returns>
        public async Task<IActionResult> DeliveredOrders()
        {
            List<UserOrder> orders = await _context.UserOrders.Where(o => o.State.HasValue && o.State == OrderState.Delivered)
               .Include(o => o.User)
               .Include(o => o.OrderProducts)
               .ThenInclude(op => op.Product)
               .AsNoTracking().ToListAsync();
            return View(orders);
        }

        /// <summary>
        /// Sets Order <see cref="UserOrder.State"/> to <see cref="OrderState.Sent"/>.
        /// </summary>
        /// <returns>JSON response with new <c>response.newOrderState</c>.</returns>
        [HttpPost]
        public async Task<JsonResult> SetOrderSent(int? orderId)
        {
            if (orderId.HasValue)
            {
                UserOrder userOrder = await _context.UserOrders.FindAsync(orderId);

                if (userOrder.State == OrderState.Pending)
                {
                    userOrder.State = OrderState.Sent;
                }
                await _context.SaveChangesAsync();
                return Json(new { newOrderState = OrderState.Sent.ToString() });
            }
            return Json(new { newOrderState = (string)null });
        }

    }
}
