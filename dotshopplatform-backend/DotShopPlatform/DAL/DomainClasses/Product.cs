using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotShopPlatform.DAL.DomainClasses
{
	/// <summary>
	/// Represents a product available for purchase.
	/// </summary>
	public class Product
	{
		/// <summary>
		/// Unique identifier for the product.
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		/// <summary>
		/// Navigation property for the associated brand.
		/// </summary>
		[ForeignKey("BrandId")]
		public virtual Brand Brand { get; set; }

		/// <summary>
		/// The identifier for the brand of this product.
		/// </summary>
		[Required]
		public int BrandId { get; set; }

		/// <summary>
		/// The name of the product. This field is required.
		/// </summary>
		[Required]
		[StringLength(255)]
		public string ProductName { get; set; }

		/// <summary>
		/// The name of the image file associated with the product.
		/// </summary>
		[Required]
		[StringLength(255)]
		public string GraphicName { get; set; }

		/// <summary>
		/// The cost price of the product. Stored as money in the database.
		/// </summary>
		[Column(TypeName = "money")]
		[DataType(DataType.Currency)]
		public decimal CostPrice { get; set; }

		/// <summary>
		/// The manufacturer's suggested retail price. Stored as money in the database.
		/// </summary>
		[Column(TypeName = "money")]
		[DataType(DataType.Currency)]
		public decimal MSRP { get; set; }

		/// <summary>
		/// The quantity of the product available on hand.
		/// </summary>
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Quantity on hand cannot be negative.")]
		public int QtyOnHand { get; set; }

		/// <summary>
		/// The quantity of the product on back order.
		/// </summary>
		[Range(0, int.MaxValue, ErrorMessage = "Quantity on back order cannot be negative.")]
		public int QtyOnBackOrder { get; set; }

		/// <summary>
		/// The description of the product.
		/// </summary>
		[Required]
		[StringLength(2000)]
		public string Description { get; set; }

		/// <summary>
		/// Timestamp used for concurrency checking.
		/// </summary>
		[Timestamp]
		public byte[] Timer { get; set; }
	}
}
