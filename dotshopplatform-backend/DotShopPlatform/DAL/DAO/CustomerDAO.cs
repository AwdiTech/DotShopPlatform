using DotShopPlatform.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotShopPlatform.DAL.DAO
{
	/// <summary>
	/// Data Access Object for handling operations related to the Customer entity.
	/// </summary>
	public class CustomerDAO
	{
		private readonly AppDbContext _db;
		private readonly ILogger<CustomerDAO> _logger;

		public CustomerDAO(AppDbContext ctx, ILogger<CustomerDAO> logger)
		{
			_db = ctx;
			_logger = logger;
		}

		/// <summary>
		/// Registers a new customer in the database.
		/// </summary>
		/// <param name="user">The customer to register.</param>
		/// <returns>The registered customer with a database-generated ID.</returns>
		public async Task<Customer> Register(Customer user)
		{
			_logger.LogInformation("Registering new customer: {Email}", user.Email);
			await _db.Customers.AddAsync(user);
			await _db.SaveChangesAsync();
			return user;
		}

		/// <summary>
		/// Retrieves a customer by their email address.
		/// </summary>
		/// <param name="email">The email address to search for.</param>
		/// <returns>The customer if found; otherwise, null.</returns>
		public async Task<Customer?> GetByEmail(string email)
		{
			_logger.LogInformation("Fetching customer by email: {Email}", email);
			Customer? user = await _db.Customers.FirstOrDefaultAsync(u => u.Email == email);
			if (user == null)
			{
				_logger.LogWarning("Customer not found for email: {Email}", email);
			}
			return user;
		}

	}
}

