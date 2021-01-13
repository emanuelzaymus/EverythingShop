﻿using EverythingShop.WebApp.Areas.Identity.Data;
using EverythingShop.WebApp.Data;
using EverythingShop.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EverythingShop.WebApp.Services
{
    public class UserOrdersService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserOrdersService(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        internal async Task<string> SetOrderDelivered(ClaimsPrincipal claims, int orderId)
        {
            AppUser user = await GetUserAsync(claims);
            UserOrder userOrder = await _context.UserOrders
                .Where(o => o.UserId == user.Id && o.Id == orderId && o.State == OrderState.Sent)
                .FirstOrDefaultAsync();

            if (userOrder != null)
            {
                userOrder.State = OrderState.Delivered;
                await _context.SaveChangesAsync();
                return OrderState.Delivered.ToString();
            }
            return null;
        }

        internal async Task CompleteOrder(UserOrder order)
        {
            order.OrderedOn = DateTime.Now;
            order.State = OrderState.Pending;

            _context.Update(order);
            await _context.SaveChangesAsync();
        }

        internal async Task<int> GetQuantityOfProductInCart(ClaimsPrincipal claims, int productId)
        {
            AppUser user = await GetUserAsync(claims);
            UserOrder userOrder = await GetCurrentOrderAsync(user, includeProducts: true, asNoTracking: true);
            if (userOrder != null)
            {
                OrderProduct orderProduct = userOrder.OrderProducts.Where(op => op.ProductId == productId).FirstOrDefault();
                if (orderProduct != null)
                {
                    return orderProduct.Quantity;
                }
            }
            return 0;
        }

        internal async Task<int> AddProductToCart(ClaimsPrincipal claims, int productId)
        {
            UserOrder userOrder = await GetCurrentOrNewOrder(claims);
            OrderProduct orderProduct = userOrder.OrderProducts.Where(op => op.ProductId == productId).FirstOrDefault();
            if (orderProduct != null)
            {
                orderProduct.Quantity++;
            }
            else if (await _context.Products.AnyAsync(p => p.Id == productId && !p.Deleted))
            {
                orderProduct = new OrderProduct() { ProductId = productId, Quantity = 1 };
                userOrder.OrderProducts.Add(orderProduct);
            }
            else return -1;

            await _context.SaveChangesAsync();
            return orderProduct.Quantity;
        }

        internal async Task<int> RemoveProductFromCart(ClaimsPrincipal claims, int productId)
        {
            int newQuantity = 0;

            AppUser user = await GetUserAsync(claims);
            UserOrder userOrder = await GetCurrentOrderAsync(user, includeProducts: true, asNoTracking: false);

            if (userOrder != null)
            {
                OrderProduct orderProduct = userOrder.OrderProducts.Where(op => op.ProductId == productId).FirstOrDefault();
                if (orderProduct != null)
                {
                    if (orderProduct.Quantity <= 1)
                        userOrder.OrderProducts.Remove(orderProduct);
                    else
                        newQuantity = --orderProduct.Quantity;

                    await _context.SaveChangesAsync();
                }
            }
            return newQuantity;
        }

        internal async Task<UserOrder> GetCurrentOrderWithoutProducts(ClaimsPrincipal claims)
        {
            var user = await GetUserAsync(claims);
            return await GetCurrentOrderAsync(user, includeProducts: false);
        }

        internal async Task<UserOrder> GetCurrentOrderWithProducts(ClaimsPrincipal claims)
        {
            var user = await GetUserAsync(claims);
            return await GetCurrentOrderAsync(user, includeProducts: true);
        }

        internal async Task<List<UserOrder>> GetFinishedOrders(ClaimsPrincipal claims)
        {
            var user = await GetUserAsync(claims);

            return await _context.UserOrders
                .Where(o => o.UserId == user.Id && o.OrderedOn != null)
                .Include(o => o.OrderProducts).ThenInclude(x => x.Product).ToListAsync();
        }

        internal async Task<UserOrder> GetCurrentOrNewOrder(ClaimsPrincipal claims)
        {
            var user = await GetUserAsync(claims);
            var currentOrder = await GetCurrentOrderAsync(user, includeProducts: true);
            if (currentOrder == null)
            {
                await CreateNewOrderAsync(user);
                currentOrder = await GetCurrentOrderAsync(user, includeProducts: true);
            }
            return currentOrder;
        }

        internal async Task<bool> IsCurrentOrderAsync(ClaimsPrincipal claims, UserOrder order)
        {
            if (order != null)
            {
                AppUser user = await GetUserAsync(claims);
                UserOrder currentOrder = await GetCurrentOrderAsync(user, includeProducts: false, asNoTracking: true);

                return currentOrder != null && order.Id == currentOrder.Id;
            }
            return false;
        }

        internal async Task<bool> IsCurrentOrderEmpty(ClaimsPrincipal claims)
        {
            AppUser user = await GetUserAsync(claims);
            UserOrder currentOrder = await GetCurrentOrderAsync(user, includeProducts: true, asNoTracking: true);

            return currentOrder == null || currentOrder.OrderProducts.Count == 0;
        }

        private async Task CreateNewOrderAsync(AppUser user)
        {
            await _context.UserOrders.AddAsync(new UserOrder() { User = user });
            if (await _context.SaveChangesAsync() < 1)
            {
                throw new Exception("New UserOrder was not saved.");
            }
        }

        private async Task<UserOrder> GetCurrentOrderAsync(AppUser user, bool includeProducts, bool asNoTracking = false)
        {
            var userOrders = _context.UserOrders.Where(o => o.UserId == user.Id && o.OrderedOn == null);

            if (includeProducts)
                userOrders = userOrders.Include(o => o.OrderProducts).ThenInclude(x => x.Product);

            if (asNoTracking)
                userOrders = userOrders.AsNoTracking();

            List<UserOrder> orders = await userOrders.ToListAsync();

            if (orders.Count > 1)
                throw new Exception("There is more than one unfinished/not-ordered order.");

            if (orders.Count == 1)
            {
                return orders[0];
            }
            return null;
        }

        private async Task<AppUser> GetUserAsync(ClaimsPrincipal claims)
        {
            return await _userManager.GetUserAsync(claims);
        }
    }
}
