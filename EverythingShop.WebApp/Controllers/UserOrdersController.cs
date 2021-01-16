using EverythingShop.WebApp.Data;
using EverythingShop.WebApp.Models;
using EverythingShop.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Controllers
{
    /// <summary>
    /// Controller for userOrders. Only for authorized user.
    /// </summary>
    [Authorize]
    public class UserOrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserOrdersService _ordersService;

        /// <summary>
        /// Creates userOrderController.
        /// </summary>
        /// <param name="context">Application DB Context</param>
        /// <param name="ordersService">user orders service</param>
        public UserOrdersController(AppDbContext context, UserOrdersService ordersService)
        {
            _context = context;
            _ordersService = ordersService;
        }

        /// <summary>
        /// User's completed orders.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            List<UserOrder> orders = await _ordersService.GetFinishedOrders(User);
            return View(orders);
        }

        /// <summary>
        /// User's Cart content.
        /// </summary>
        public async Task<IActionResult> Cart()
        {
            UserOrder cartContent = (await _ordersService.GetCurrentOrNewOrder(User));
            return View(cartContent);
        }

        /// <summary>
        /// Request for completing current user order.
        /// </summary>
        /// <returns>CompleteOrder View if such a order exists. Else NotFound.</returns>
        public async Task<IActionResult> CompleteOrder()
        {
            UserOrder order = await _ordersService.GetCurrentOrderWithProducts(User);
            if (order == null || order.OrderProducts.Count == 0)
            {
                return NotFound();
            }
            return View(order);
        }

        /// <summary>
        /// Completes order.
        /// </summary>
        /// <param name="order">Order address data in <see cref="UserOrder"/> object.</param>
        /// <returns>If success redirects to Index else not.</returns>
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

        /// <summary>
        /// API POST Request.
        /// Set Order based on <paramref name="orderId"/> <see cref="OrderState.Delivered"/>.
        /// </summary>
        /// <param name="orderId">User order ID to set Delivered.</param>
        /// <returns>JSON obect with new order state <c>response.newOrderState</c>.</returns>
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
