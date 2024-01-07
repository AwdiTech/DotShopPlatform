using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotShopPlatform.DAL.DomainClasses
{
	/// <summary>
	/// Represents an order placed by a customer.
	/// </summary>
	public class Order
	{
		/// <summary>
		/// Constructs an order with an empty set of order line items.
		/// </summary>
		public Order()
		{
			OrderLineItems = new HashSet<OrderLineItem>();
		}

		/// <summary>
		/// Unique identifier for the order.
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		/// <summary>
		/// The date and time when the order was placed.
		/// </summary>
		[Required]
		public DateTime OrderDate { get; set; }

		/// <summary>
		/// The total amount of the order. Stored as money in the database.
		/// </summary>
		[Column(TypeName = "money")]
		[DataType(DataType.Currency)]
		public decimal OrderAmount { get; set; }

		/// <summary>
		/// The customer identifier for the order. This creates a foreign key relationship.
		/// </summary>
		[Required]
		public int UserId { get; set; }

		/// <summary>
		/// Collection of order line items associated with this order.
		/// </summary>
		public virtual ICollection<OrderLineItem> OrderLineItems { get; set; }
	}
}

