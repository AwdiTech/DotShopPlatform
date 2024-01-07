using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotShopPlatform.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotShopPlatform.DAL.DAO
{
	/// <summary>
	/// Data Access Object for handling operations related to the Product entity.
	/// </summary>
	public class ProductDAO
	{
		private readonly AppDbContext _db;
		private readonly ILogger _logger;

		public ProductDAO(AppDbContext ctx, ILogger<ProductDAO> logger)
		{
			_db = ctx;
			_logger = logger;
		}

		/// <summary>
		/// Retrieves all products associated with a specific brand.
		/// </summary>
		/// <param name="brandId">The ID of the brand.</param>
		/// <returns>A list of products for the specified brand.</returns>
		public async Task<List<Product>> GetAllByBrand(int brandId)
		{
			_logger.LogInformation("Fetching all products for brand: {BrandId}", brandId);
			try
			{
				return await _db.Products
								.Where(product => product.BrandId == brandId)
								.ToListAsync();
			}
			catch (System.Exception ex)
			{
				_logger.LogError(ex, "Error fetching products for brand: {BrandId}", brandId);
				throw;
			}
		}

		/// <summary>
		/// Retrieves a product by its unique identifier.
		/// </summary>
		/// <param name="productId">The product's unique identifier.</param>
		/// <returns>The product if found; otherwise, null.</returns>
		public async Task<Product?> GetById(string productId)
		{
			_logger.LogInformation("Fetching product by ID: {ProductId}", productId);
			try
			{
				Product? product = await _db.Products
											.FirstOrDefaultAsync(p => p.Id == productId);
				if (product == null)
				{
					_logger.LogWarning("Product not found for ID: {ProductId}", productId);
				}
				return product;
			}
			catch (System.Exception ex)
			{
				_logger.LogError(ex, "Error fetching product by ID: {ProductId}", productId);
				throw;
			}
		}
	}
}
