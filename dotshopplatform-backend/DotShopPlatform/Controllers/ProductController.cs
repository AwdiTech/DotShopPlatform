using System.Collections.Generic;
using System.Threading.Tasks;
using DotShopPlatform.DAL;
using DotShopPlatform.DAL.DAO;
using DotShopPlatform.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotShopPlatform.Controllers
{
	/// <summary>
	/// Controller for handling product-related operations.
	/// </summary>
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly AppDbContext _db;
		private readonly ILogger<ProductController> _logger;
		private readonly ILogger<ProductDAO> _productDaoLogger;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProductController"/> class.
		/// </summary>
		/// <param name="context">The database context.</param>
		/// <param name="logger">The logger.</param>
		public ProductController(AppDbContext context, ILogger<ProductController> logger, ILogger<ProductDAO> productDaoLogger)
		{
			_db = context;
			_logger = logger;
			_productDaoLogger = productDaoLogger;
		}

		/// <summary>
		/// Retrieves all products associated with a given brand.
		/// </summary>
		/// <param name="brandid">The brand identifier.</param>
		/// <returns>A list of products.</returns>
		[HttpGet("{brandid}")]
		public async Task<ActionResult<List<Product>>> Index(int brandid)
		{
			_logger.LogInformation("Fetching products for brand id: {BrandId}", brandid);
			try
			{
				ProductDAO dao = new ProductDAO(_db, _productDaoLogger);
				List<Product> itemsForBrand = await dao.GetAllByBrand(brandid);
				return Ok(itemsForBrand);
			}
			catch (System.Exception ex)
			{
				_logger.LogError(ex, "Error fetching products for brand id: {BrandId}", brandid);
				return StatusCode(500, "Internal server error");
			}
		}
	}
}
