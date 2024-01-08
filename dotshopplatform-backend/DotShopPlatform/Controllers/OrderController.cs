using System.Threading.Tasks;
using DotShopPlatform.APIHelpers;
using DotShopPlatform.DAL;
using DotShopPlatform.DAL.DAO;
using DotShopPlatform.DAL.DomainClasses;
using DotShopPlatform.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotShopPlatform.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly AppDbContext _ctx;
		private readonly ILogger<OrderController> _logger;
		private readonly ILogger<CustomerDAO> _customerDaoLogger;
		private readonly ILogger<OrderDAO> _orderDaoLogger;

		public OrderController(AppDbContext context, ILogger<OrderController> logger, ILogger<CustomerDAO> customerDaoLogger, ILogger<OrderDAO> orderDaoLogger)
		{
			_ctx = context;
			_logger = logger;
			_customerDaoLogger = customerDaoLogger;
			_orderDaoLogger = orderDaoLogger;
		}

		[HttpPost]
		[Produces("application/json")]
		public async Task<ActionResult<string>> Index(OrderHelper helper)
		{
			string retVal;
			try
			{
				CustomerDAO cDao = new CustomerDAO(_ctx, _customerDaoLogger);
				Customer orderOwner = await cDao.GetByEmail(helper.Email);
				if (orderOwner == null)
				{
					return BadRequest("Order could not be placed: customer not found.");
				}

				var oDao = new OrderDAO(_ctx, _orderDaoLogger);
				int orderId = await oDao.AddOrder(orderOwner.Id, helper.Selections);

				retVal = orderId > 0 ? $"Order {orderId} saved!" : "Order not saved";
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error in placing order");
				retVal = $"Order not saved due to error: {ex.Message}";
			}
			return new JsonResult(new { message = retVal });
		}

		[Route("{email}")]
		public async Task<ActionResult<List<Order>>> List(string email)
		{
			try
			{
				var cDao = new CustomerDAO(_ctx, _customerDaoLogger);
				var orderOwner = await cDao.GetByEmail(email);
				if (orderOwner == null)
				{
					return NotFound("Customer not found.");
				}

				var oDao = new OrderDAO(_ctx, _orderDaoLogger);
				var orders = await oDao.GetAll(orderOwner.Id);

				return orders;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error in fetching orders");
				return StatusCode(500, "Error fetching orders");
			}
		}

		[Route("{orderId}/{email}")]
		public async Task<ActionResult<List<OrderDetailsHelper>>> GetOrderDetails(int orderId, string email)
		{
			try
			{
				var dao = new OrderDAO(_ctx, _orderDaoLogger);
				var orderDetails = await dao.GetOrderDetails(orderId, email);

				return orderDetails.Count > 0 ? orderDetails : NotFound("Order details not found.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error in fetching order details");
				return StatusCode(500, "Error fetching order details");
			}
		}
	}
}
