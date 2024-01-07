namespace DotShopPlatform.APIHelpers
{
	/// <summary>
	/// Helper class to transfer order data in API calls.
	/// </summary>
	public class OrderHelper
	{
		/// <summary>
		/// Gets or sets the email address associated with the order.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets an array of order selection details.
		/// </summary>
		public OrderSelectionHelper[] Selections { get; set; }
	}
}
