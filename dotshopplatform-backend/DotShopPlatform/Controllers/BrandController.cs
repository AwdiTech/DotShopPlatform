using DotShopPlatform.DAL;
using DotShopPlatform.DAL.DAO;
using DotShopPlatform.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotShopPlatform.Controllers
{
	/// <summary>
	/// Controller for handling Brand-related operations.
	/// </summary>
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class BrandController : ControllerBase
	{
		private readonly AppDbContext _db;
		private readonly ILogger<BrandDAO> _brandDaoLogger;

		/// <summary>
		/// Initializes a new instance of the <see cref="BrandController"/> class.
		/// </summary>
		/// <param name="context">Database context.</param>
		/// <param name="brandDaoLogger">Logger for BrandDAO.</param>
		public BrandController(AppDbContext context, ILogger<BrandDAO> brandDaoLogger)
		{
			_db = context;
			_brandDaoLogger = brandDaoLogger;
		}

		/// <summary>
		/// Retrieves all brands.
		/// </summary>
		/// <returns>A list of brands.</returns>
		[HttpGet]
		public async Task<ActionResult<List<Brand>>> Index()
		{
			try
			{
				BrandDAO dao = new BrandDAO(_db, _brandDaoLogger);
				List<Brand> allBrands = await dao.GetAllAsync();
				if (allBrands == null || allBrands.Count == 0)
				{
					_brandDaoLogger.LogInformation("No brands found");
					return NotFound("No brands available");
				}
				return allBrands;
			}
			catch (Exception ex)
			{
				_brandDaoLogger.LogError($"Error in BrandController Index: {ex.Message}");
				return StatusCode(500, "Internal server error, please try again later.");
			}
		}
	}
}
