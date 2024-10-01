namespace Entities;

[PrimaryKey(nameof(UserId), nameof(Title))]
public class ProductOrService : IEntity
{
	[ForeignKey(nameof(User))]
	public int UserId { get; set; }
	public required string Title { get; set; }
	public long? DefaultPriceForEach { get; set; }
	public int DefaultQuantity { get; set; } = 1;
	public byte DefaultTaxPercent { get; set; }
	[StringLength(20)]
	public string? BarCode { get; set; }

	public User? User { get; set; }
}