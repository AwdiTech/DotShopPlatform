using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotShopPlatform.DAL.DomainClasses
{
	/// <summary>
	/// Represents a brand entity in the system.
	/// </summary>
	public class Brand
	{
		/// <summary>
		/// Unique identifier for the brand.
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		/// <summary>
		/// The name of the brand. This field is required and has a max length of 200 characters.
		/// </summary>
		[Required]
		[StringLength(200)]
		public string Name { get; set; }

		/// <summary>
		/// Timestamp used for concurrency checking.
		/// </summary>
		[Timestamp]
		public byte[] Timer { get; set; }
	}
}