/*using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DotShopPlatform.DAL;
using DotShopPlatform.DAL.DAO;
using DotShopPlatform.DAL.DomainClasses;
using DotShopPlatform.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotShopPlatform.Controllers
{
	/// <summary>
	/// Controller for handling customer registration operations.
	/// </summary>
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class RegisterController : ControllerBase
	{
		private readonly AppDbContext _db;
		private readonly ILogger<RegisterController> _logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="RegisterController"/> class.
		/// </summary>
		/// <param name="context">The database context.</param>
		/// <param name="logger">The logger.</param>
		public RegisterController(AppDbContext context, ILogger<RegisterController> logger)
		{
			_db = context;
			_logger = logger;
		}

		/// <summary>
		/// Registers a new customer.
		/// </summary>
		/// <param name="helper">The customer registration helper.</param>
		/// <returns>A response indicating the registration result.</returns>
		[AllowAnonymous]
		[HttpPost]
		[Produces("application/json")]
		public async Task<ActionResult<CustomerHelper>> Index(CustomerHelper helper)
		{
			try
			{
				CustomerDAO dao = new CustomerDAO(_db, _logger);
				Customer existingCustomer = await dao.GetByEmail(helper.Email);
				if (existingCustomer == null)
				{
					HashSalt hashSalt = GenerateSaltedHash(64, helper.Password);
					helper.Password = ""; // Clearing the password

					Customer newCustomer = new Customer
					{
						Firstname = helper.Firstname,
						Lastname = helper.Lastname,
						Email = helper.Email,
						Hash = hashSalt.Hash,
						Salt = hashSalt.Salt
					};

					newCustomer = await dao.Register(newCustomer);
					helper.Token = newCustomer.Id > 0 ? "Customer registered" : "Customer registration failed";
				}
				else
				{
					helper.Token = "Customer registration failed - email already in use";
				}

				return helper;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error in registering customer.");
				return StatusCode(500, "Internal server error");
			}
		}

		private static HashSalt GenerateSaltedHash(int size, string password)
		{
			var saltBytes = new byte[size];
			var provider = new RNGCryptoServiceProvider();
			provider.GetNonZeroBytes(saltBytes);
			var salt = Convert.ToBase64String(saltBytes);
			var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
			var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
			return new HashSalt { Hash = hashPassword, Salt = salt };
		}
	}

	public class HashSalt
	{
		public string Hash { get; set; }
		public string Salt { get; set; }
	}
}*/

using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DotShopPlatform.DAL;
using DotShopPlatform.DAL.DAO;
using DotShopPlatform.DAL.DomainClasses;
using DotShopPlatform.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;

namespace DotShopPlatform.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class RegisterController : ControllerBase
	{
		private readonly AppDbContext _db;
		private readonly ILogger<RegisterController> _logger;
		private readonly ILogger<CustomerDAO> _customerDaoLogger;

		public RegisterController(AppDbContext context, ILogger<RegisterController> logger, ILogger<CustomerDAO> customerDaoLogger)
		{
			_db = context;
			_logger = logger;
			_customerDaoLogger = customerDaoLogger;
		}

		[AllowAnonymous]
		[HttpPost]
		[Produces("application/json")]
		public async Task<ActionResult<CustomerHelper>> Index(CustomerHelper helper)
		{
			try
			{
				CustomerDAO dao = new CustomerDAO(_db, _customerDaoLogger);
				Customer? existingCustomer = await dao.GetByEmail(helper.Email);
				if (existingCustomer == null)
				{
					HashSalt hashSalt = GenerateSaltedHash(64, helper.Password);
					helper.Password = "";

					Customer newCustomer = new Customer
					{
						Firstname = helper.Firstname,
						Lastname = helper.Lastname,
						Email = helper.Email,
						Hash = hashSalt.Hash,
						Salt = hashSalt.Salt
					};

					newCustomer = await dao.Register(newCustomer);
					helper.Token = newCustomer.Id > 0 ? "Customer registered" : "Customer registration failed";
				}
				else
				{
					helper.Token = "Customer registration failed - email already in use";
				}

				return helper;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error in registering customer.");
				return StatusCode(500, "Internal server error");
			}
		}

		private static HashSalt GenerateSaltedHash(int size, string password)
		{
			var saltBytes = RandomNumberGenerator.GetBytes(size);
			var salt = Convert.ToBase64String(saltBytes);
			var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
			var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
			return new HashSalt { Hash = hashPassword, Salt = salt };
		}
	}

	public class HashSalt
	{
		public string? Hash { get; set; }
		public string? Salt { get; set; }
	}
}
