using DotShopPlatform.DAL.DomainClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotShopPlatform.DAL.DomainClasses
{
	/// <summary>
	/// Represents an item within an order, detailing the product and quantities involved.
	/// </summary>
	public class OrderLineItem
	{
		/// <summary>
		/// Unique identifier for the order line item.
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		/// <summary>
		/// The identifier of the order this line item belongs to.
		/// </summary>
		[Required]
		public int OrderId { get; set; }

		/// <summary>
		/// The quantity of the product that was ordered.
		/// </summary>
		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Quantity ordered must be at least 1.")]
		public int QtyOrdered { get; set; }

		/// <summary>
		/// The quantity of the product that has been sold.
		/// </summary>
		[Required]
		public int QtySold { get; set; }

		/// <summary>
		/// The quantity of the product that is back-ordered.
		/// </summary>
		[Required]
		public int QtyBackOrdered { get; set; }

		/// <summary>
		/// The selling price of the product. Stored as money in the database.
		/// </summary>
		[Column(TypeName = "money")]
		[DataType(DataType.Currency)]
		public decimal SellingPrice { get; set; }

		/// <summary>
		/// Navigation property for the associated order.
		/// </summary>
		[ForeignKey("OrderId")]
		public virtual Order Order { get; set; }

		/// <summary>
		/// The identifier of the product in this line item.
		/// </summary>
		[Required]
		public string ProductId { get; set; }

		/// <summary>
		/// Navigation property for the associated product.
		/// </summary>
		[ForeignKey("ProductId")]
		public virtual Product Product { get; set; }
	}
}
