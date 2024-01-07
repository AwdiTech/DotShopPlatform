using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotShopPlatform.DAL.DomainClasses
{
	/// <summary>
	/// Represents a customer entity in the system.
	/// </summary>
	public class Customer
	{
		/// <summary>
		/// Unique identifier for the customer.
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		/// <summary>
		/// The first name of the customer. This field is required.
		/// </summary>
		[Required]
		[StringLength(50)]
		public string Firstname { get; set; }

		/// <summary>
		/// The last name of the customer. This field is required.
		/// </summary>
		[Required]
		[StringLength(50)]
		public string Lastname { get; set; }

		/// <summary>
		/// The email address of the customer. This field is required and used for identification.
		/// </summary>
		[Required]
		[EmailAddress]
		[StringLength(255)]
		public string Email { get; set; }

		/// <summary>
		/// The hash of the customer's password. This field is required and used for authentication.
		/// </summary>
		[Required]
		public string Hash { get; set; }

		/// <summary>
		/// The salt used in conjunction with the hash. This field is required and used for authentication.
		/// </summary>
		[Required]
		public string Salt { get; set; }
	}
}