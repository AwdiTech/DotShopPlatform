using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotShopPlatform.DAL.DomainClasses;
using DotShopPlatform.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotShopPlatform.DAL.DAO
{
	/// <summary>
	/// Data Access Object for handling operations related to the Order entity.
	/// </summary>
	public class OrderDAO
	{
		private readonly AppDbContext _db;
		private readonly ILogger _logger;

		public OrderDAO(AppDbContext ctx, ILogger<OrderDAO> logger)
		{
			_db = ctx;
			_logger = logger;
		}

		/// <summary>
		/// Adds an order to the database.
		/// </summary>
		/// <param name="userid">The ID of the user placing the order.</param>
		/// <param name="selections">The items selected for the order.</param>
		/// <returns>The ID of the created order, or -1 if the operation fails.</returns>
		public async Task<int> AddOrder(int userid, OrderSelectionHelper[] selections)
		{
			int orderId = -1;
			using (_db)
			{
				using (var _trans = await _db.Database.BeginTransactionAsync())
				{
					try
					{
						Order order = new Order
						{
							UserId = userid,
							OrderDate = System.DateTime.Now,
							OrderAmount = 0
						};

						foreach (var selection in selections)
						{
							order.OrderAmount += selection.product.MSRP * selection.Qty;
						}

						await _db.Orders.AddAsync(order);
						await _db.SaveChangesAsync();

						foreach (var selection in selections)
						{
							OrderLineItem oItem = new OrderLineItem
							{
								ProductId = selection.product.Id,
								OrderId = order.Id,
								SellingPrice = selection.product.MSRP,
								QtyOrdered = selection.Qty
							};

							var prod = await _db.Products.FindAsync(selection.product.Id);
							if (prod != null)
							{
								oItem.QtySold = Math.Min(oItem.QtyOrdered, prod.QtyOnHand);
								oItem.QtyBackOrdered = Math.Max(0, oItem.QtyOrdered - prod.QtyOnHand);
								prod.QtyOnHand = Math.Max(0, prod.QtyOnHand - oItem.QtyOrdered);
								await _db.OrderLineItems.AddAsync(oItem);
							}
						}

						await _db.SaveChangesAsync();
						await _trans.CommitAsync();
						orderId = order.Id;
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "Error adding order for user {UserId}", userid);
						await _trans.RollbackAsync();
					}
				}
			}
			return orderId;
		}

		/// <summary>
		/// Adds an order to the database.
		/// </summary>
		/// <param name="userid">The ID of the user placing the order.</param>
		/// <param name="selections">The items selected for the order.</param>
		/// <returns>The ID of the created order, or -1 if the operation fails.</returns>

		/// <summary>
		/// Retrieves all orders made by a specific user.
		/// </summary>
		/// <param name="id">The user's ID.</param>
		/// <returns>A list of orders.</returns>
		public async Task<List<Order>> GetAll(int id)
		{
			_logger.LogInformation("Fetching all orders for user: {UserId}", id);
			return await _db.Orders.Where(order => order.UserId == id).ToListAsync();
		}

		/// <summary>
		/// Retrieves the details of a specific order.
		/// </summary>
		/// <param name="oid">The order ID.</param>
		/// <param name="email">The email of the user who placed the order.</param>
		/// <returns>A list of order details.</returns>
		public async Task<List<OrderDetailsHelper>> GetOrderDetails(int oid, string email)
		{
			_logger.LogInformation("Fetching order details for order: {OrderId} and email: {Email}", oid, email);
			var results = from o in _db.Orders
						  join oi in _db.OrderLineItems on o.Id equals oi.OrderId
						  join p in _db.Products on oi.ProductId equals p.Id
						  where (o.UserId == oid)
						  select new OrderDetailsHelper
						  {
							  OrderId = o.Id,
							  UserId = o.UserId,
							  QtyOrdered = oi.QtyOrdered,
							  QtyBackOrdered = oi.QtyBackOrdered,
							  QtySold = oi.QtySold,
							  ProductName = p.ProductName,
							  ProductId = oi.ProductId,
							  DateCreated = o.OrderDate.ToString("yyyy/MM/dd - hh:mm tt")
						  };
			return await results.ToListAsync();
		}
	}
}
