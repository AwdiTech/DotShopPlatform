using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DotShopPlatform.DAL;
using DotShopPlatform.DAL.DAO;
using DotShopPlatform.DAL.DomainClasses;
using DotShopPlatform.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace DotShopPlatform.Controllers
{
	/// <summary>
	/// Controller for handling user login requests.
	/// </summary>
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly AppDbContext _db;
		private readonly IConfiguration _configuration;
		private readonly ILogger<LoginController> _logger;
		private readonly ILogger<CustomerDAO> _customerDaoLogger;

		/// <summary>
		/// Initializes a new instance of the <see cref="LoginController"/> class.
		/// </summary>
		/// <param name="context">Database context for the application.</param>
		/// <param name="configuration">Configuration for the application.</param>
		/// <param name="logger">Logger for the LoginController.</param>
		public LoginController(AppDbContext context, IConfiguration configuration, ILogger<LoginController> logger, ILogger<CustomerDAO> customerDaoLogger)
		{
			_db = context;
			_configuration = configuration;
			_logger = logger;
			_customerDaoLogger = customerDaoLogger;
		}

		/// <summary>
		/// Authenticates a user and returns a JWT token if successful.
		/// </summary>
		/// <param name="helper">Helper class containing user credentials.</param>
		/// <returns>A JWT token upon successful authentication or an error message.</returns>
		[AllowAnonymous]
		[HttpPost]
		[Produces("application/json")]
		public async Task<ActionResult<CustomerHelper>> Index(CustomerHelper helper)
		{
			CustomerDAO dao = new CustomerDAO(_db, _customerDaoLogger);
			Customer customer = await dao.GetByEmail(helper.Email);
			if (customer != null)
			{
				if (VerifyPassword(helper.Password, customer.Hash, customer.Salt))
				{
					helper.Password = "";
					var secretKey = _configuration.GetValue<string>("AppSettings:Secret");
					var token = GenerateJwtToken(customer.Id, secretKey);
					helper.Token = token;
				}
				else
				{
					_logger.LogWarning("Login failed for email: {Email}, invalid password", helper.Email);
					helper.Token = "Username or password invalid - login failed";
				}
			}
			else
			{
				_logger.LogWarning("Login failed for email: {Email}, user not found", helper.Email);
				helper.Token = "No such customer - login failed";
			}
			return helper;
		}

		public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
		{
			var saltBytes = Convert.FromBase64String(storedSalt);
			var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000, HashAlgorithmName.SHA256);
			return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
		}

		private string GenerateJwtToken(int userId, string secretKey)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(secretKey);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.Name, userId.ToString())
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
