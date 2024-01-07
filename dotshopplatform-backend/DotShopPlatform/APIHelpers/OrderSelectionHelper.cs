using DotShopPlatform.DAL.DomainClasses;

namespace DotShopPlatform.APIHelpers
{
	/// <summary>
	/// Helper class for transferring details of a selected product in an order.
	/// </summary>
	public class OrderSelectionHelper
	{
		/// <summary>
		/// Gets or sets the quantity of the selected product.
		/// </summary>
		public int Quantity { get; set; }

		/// <summary>
		/// Gets or sets the product details for the order selection.
		/// </summary>
		public Product Product { get; set; }
	}
}
