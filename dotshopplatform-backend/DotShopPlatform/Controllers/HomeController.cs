using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotShopPlatform.Controllers
{
	/// <summary>
	/// Controller for handling requests to the home route of the API.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class HomeController : ControllerBase
	{
		private readonly ILogger<HomeController> _logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="HomeController"/> class.
		/// </summary>
		/// <param name="logger">Logger for the HomeController.</param>
		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// Responds to a GET request by indicating that the server has started.
		/// </summary>
		/// <returns>A message indicating that the server has started.</returns>
		[HttpGet]
		public ActionResult<string> Index()
		{
			_logger.LogInformation("Home controller accessed at {Time}", DateTime.UtcNow);
			return "Server started";
		}
	}
}
