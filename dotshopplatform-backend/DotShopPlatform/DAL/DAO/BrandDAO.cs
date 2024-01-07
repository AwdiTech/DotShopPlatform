using DotShopPlatform.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace DotShopPlatform.DAL.DAO
{
	public class BrandDAO
	{
		private readonly AppDbContext _db;
		private readonly ILogger<BrandDAO> _logger;

		public BrandDAO(AppDbContext ctx, ILogger<BrandDAO> logger)
		{
			_db = ctx;
			_logger = logger;
		}

		/// <summary>
		/// Retrieves all brands from the database.
		/// </summary>
		/// <returns>A list of all brands.</returns>
		public async Task<List<Brand>> GetAllAsync()
		{
			try
			{
				return await _db.Brands.ToListAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in GetAllAsync: {ex.Message}");
				throw;
			}
		}
	}
}
