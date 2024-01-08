using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DotShopPlatform.DAL.DomainClasses;
using Microsoft.Extensions.Logging;

namespace DotShopPlatform.DAL
{
	/// <summary>
	/// Utility class to load data from JSON to the database.
	/// </summary>
	public class DataUtility
	{
		private readonly AppDbContext _db;
		private readonly ILogger<DataUtility> _logger;

		/// <summary>
		/// Initializes a new instance of the DataUtility class.
		/// </summary>
		/// <param name="context">The database context.</param>
		/// <param name="logger">The logger.</param>
		public DataUtility(AppDbContext context, ILogger<DataUtility> logger)
		{
			_db = context;
			_logger = logger;
		}

		/// <summary>
		/// Loads clothing information from a JSON string into the database.
		/// </summary>
		/// <param name="stringJson">The JSON string containing clothing data.</param>
		/// <returns>A boolean value indicating whether the data was loaded successfully.</returns>
		public async Task<bool> LoadClothingInfoFromWebToDb(string stringJson)
		{
			bool brandsLoaded = false;
			bool productsLoaded = false;
			try
			{
				JsonDocument doc = JsonDocument.Parse(stringJson);
				JsonElement root = doc.RootElement;
				brandsLoaded = await LoadBrands(root);
				productsLoaded = await LoadProducts(root);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading data from JSON.");
			}
			return brandsLoaded && productsLoaded;
		}

		/// <summary>
		/// Loads brand information from a JSON element into the database.
		/// </summary>
		/// <param name="jsonElement">The JSON element containing brand data.</param>
		/// <returns>A boolean value indicating whether the brands were loaded successfully.</returns>
		private async Task<bool> LoadBrands(JsonElement jsonElement)
		{
			bool loadedBrands = false;
			try
			{
				_db.Brands.RemoveRange(_db.Brands);
				await _db.SaveChangesAsync();
				HashSet<string> allBrands = new HashSet<string>();

				foreach (JsonElement element in jsonElement.EnumerateArray())
				{
					if (element.TryGetProperty("BRAND", out JsonElement brandJson))
					{
						allBrands.Add(brandJson.GetString());
					}
				}

				foreach (string brandName in allBrands)
				{
					Brand brand = new Brand { Name = brandName };
					await _db.Brands.AddAsync(brand);
				}
				await _db.SaveChangesAsync();
				loadedBrands = true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading brands.");
			}
			return loadedBrands;
		}

		/// <summary>
		/// Loads product information from a JSON element into the database.
		/// </summary>
		/// <param name="jsonElement">The JSON element containing product data.</param>
		/// <returns>A boolean value indicating whether the products were loaded successfully.</returns>
		private async Task<bool> LoadProducts(JsonElement jsonElement)
		{
			bool loadedProducts = false;
			try
			{
				List<Brand> brands = _db.Brands.ToList();
				_db.Products.RemoveRange(_db.Products);
				await _db.SaveChangesAsync();

				foreach (JsonElement element in jsonElement.EnumerateArray())
				{
					Product product = new Product
					{
						ProductName = element.GetProperty("PRODUCTNAME").GetString(),
						GraphicName = element.GetProperty("GRAPHICNAME").GetString(),
						QtyOnHand = element.GetProperty("QTYONHAND").GetInt32(),
						CostPrice = element.GetProperty("COSTPRICE").GetDecimal(),
						Description = element.GetProperty("DESCRIPTION").GetString(),
						QtyOnBackOrder = element.TryGetProperty("QTYONBACKORDER", out JsonElement backOrder) ? backOrder.GetInt32() : 0,
						MSRP = element.TryGetProperty("MSRP", out JsonElement msrp) ? msrp.GetDecimal() : 0,
						Brand = brands.FirstOrDefault(b => b.Name == element.GetProperty("BRAND").GetString())
					};

					if (product.Brand != null)
					{
						await _db.Products.AddAsync(product);
					}
				}
				await _db.SaveChangesAsync();
				loadedProducts = true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading products.");
			}
			return loadedProducts;
		}
	}
}