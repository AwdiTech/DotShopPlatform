using Microsoft.AspNetCore.Mvc;
using DotShopPlatform.DAL;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace DotShopPlatform.Controllers
{
	/// <summary>
	/// Controller for handling data loading operations.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class DataController : ControllerBase
	{
		private readonly AppDbContext _ctx;
		private readonly ILogger<DataController> _logger;
		private readonly ILogger<DataUtility> _dataUtilityLogger;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataController"/>.
		/// </summary>
		/// <param name="context">Database context.</param>
		/// <param name="logger">Logger for DataController.</param>
		public DataController(AppDbContext context, ILogger<DataController> logger, ILogger<DataUtility> dataUtilityLogger)
		{
			_ctx = context;
			_logger = logger;
			_dataUtilityLogger = dataUtilityLogger;
		}

		/// <summary>
		/// Fetches data from a web resource and loads it into the database.
		/// </summary>
		/// <returns>A message indicating the status of the operation.</returns>
		[HttpGet]
		public async Task<ActionResult<string>> Index()
		{
			_logger.LogInformation("Starting data loading process");
			string payload;
			try
			{
				string json = await getMensClothingJsonWebAsync();
				DataUtility util = new DataUtility(_ctx, _dataUtilityLogger);
				bool isLoaded = await util.LoadClothingInfoFromWebToDb(json); // Updated method name
				payload = isLoaded ? "Tables loaded successfully" : "Problem loading tables";
			}
			catch (Exception ex)
			{
				_logger.LogError($"Data loading error: {ex.Message}");
				payload = $"Error during data loading: {ex.Message}";
			}
			return JsonSerializer.Serialize(payload);
		}

		/// <summary>
		/// Retrieves JSON data from a web resource asynchronously.
		/// </summary>
		/// <returns>The JSON string.</returns>
		private async Task<string> getMensClothingJsonWebAsync()
		{
			string url = "https://raw.githubusercontent.com/AwdiTech/DotShopPlatform/main/data/mensclothing.json";
			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.GetAsync(url);
				return await response.Content.ReadAsStringAsync();
			}
		}
	}
}
