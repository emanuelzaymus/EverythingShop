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

        internal async Task<UserOrder> GetCurrentOrder(ClaimsPrincipal claims)
        {
            var user = await GetUserAsync(claims);
            return await GetCurrentOrderAsync(user, includeProducts: false);
        }

        internal async Task<List<UserOrder>> GetFinishedOrders(ClaimsPrincipal claims)
        {
            var user = await GetUserAsync(claims);

            return await _context.UserOrders
                .Where(o => o.UserId == user.Id && o.OrderedOn != null)
                .Include(o => o.OrderProducts).ThenInclude(x => x.Product).ToListAsync();
        }

        internal async Task<List<OrderProduct>> GetCartContent(ClaimsPrincipal claims)
        {
            var user = await GetUserAsync(claims);
            var currentOrder = await GetCurrentOrderAsync(user, includeProducts: true);
            if (currentOrder == null)
            {
                await CreateNewOrderAsync(user);
                currentOrder = await GetCurrentOrderAsync(user, includeProducts: true);
            }
            return currentOrder.OrderProducts;
        }

        internal async Task<bool> IsCurrentOrderAsync(UserOrder order, ClaimsPrincipal claims)
        {
            if (order != null)
            {
                AppUser user = await GetUserAsync(claims);
                UserOrder currentOrder = await GetCurrentOrderAsync(user, includeProducts: false, asNoTracking: true);
                return order.Id == currentOrder.Id;
            }
            return false;
        }

        private async Task CreateNewOrderAsync(AppUser user)
        {
            EntityEntry<UserOrder> u = await _context.UserOrders.AddAsync(new UserOrder() { User = user });
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