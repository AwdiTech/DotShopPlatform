using Microsoft.EntityFrameworkCore;
using DotShopPlatform.DAL.DomainClasses;

namespace DotShopPlatform.DAL
{
	/// <summary>
	/// Database context for the DotShopPlatform application.
	/// </summary>
	public class AppDbContext : DbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AppDbContext"/> class.
		/// </summary>
		/// <param name="options">The options to be used by this context.</param>
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		/// <summary>
		/// Gets or sets the DbSet for Products.
		/// </summary>
		public DbSet<Product> Products { get; set; }

		/// <summary>
		/// Gets or sets the DbSet for Brands.
		/// </summary>
		public DbSet<Brand> Brands { get; set; }

		/// <summary>
		/// Gets or sets the DbSet for Customers.
		/// </summary>
		public DbSet<Customer> Customers { get; set; }

		/// <summary>
		/// Gets or sets the DbSet for Order Line Items.
		/// </summary>
		public DbSet<OrderLineItem> OrderLineItems { get; set; }

		/// <summary>
		/// Gets or sets the DbSet for Orders.
		/// </summary>
		public DbSet<Order> Orders { get; set; }
	}
}