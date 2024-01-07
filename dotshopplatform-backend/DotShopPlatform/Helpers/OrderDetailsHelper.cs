namespace DotShopPlatform.Helpers
{
	/// <summary>
	/// Helper class for representing details of an order.
	/// </summary>
	public class OrderDetailsHelper
	{
		/// <summary>
		/// Gets or sets the order identifier.
		/// </summary>
		public int OrderId { get; set; }

		/// <summary>
		/// Gets or sets the product identifier.
		/// </summary>
		public string ProductId { get; set; }

		/// <summary>
		/// Gets or sets the quantity ordered.
		/// </summary>
		public int QtyOrdered { get; set; }

		/// <summary>
		/// Gets or sets the quantity sold.
		/// </summary>
		public int QtySold { get; set; }

		/// <summary>
		/// Gets or sets the quantity back ordered.
		/// </summary>
		public int QtyBackOrdered { get; set; }

		/// <summary>
		/// Gets or sets the name of the product.
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Gets or sets the user identifier associated with the order.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the date when the order was created.
		/// </summary>
		/// <remarks>
		/// The date is stored as a string in a specific format.
		/// </remarks>
		public string DateCreated { get; set; }
	}
}
