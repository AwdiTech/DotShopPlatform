using System;

namespace DotShopPlatform.Helpers
{
	/// <summary>
	/// Helper class to transfer customer data.
	/// </summary>
	public class CustomerHelper
	{
		/// <summary>
		/// Gets or sets the first name of the customer.
		/// </summary>
		public string Firstname { get; set; }

		/// <summary>
		/// Gets or sets the last name of the customer.
		/// </summary>
		public string Lastname { get; set; }

		/// <summary>
		/// Gets or sets the email of the customer.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the password of the customer.
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the token for the customer. This may be null.
		/// </summary>
		public string? Token { get; set; }
	}
}
