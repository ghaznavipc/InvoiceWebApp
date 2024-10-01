namespace Entities;

[PrimaryKey(nameof(Id), nameof(InvoiceNumber), nameof(InvoiceUserId))]
public class Row
{
	public int Id { get; set; }
	public required string Title { get; set; }
	public long PriceForEach { get; set; }
	public int Quantity { get; set; } = 1;
	public long PriceForAll { get; private set; }
	public long Discount { get; set; }
	//public long HiddenDiscountFromTotalInvoice { get; set; } // not for now
	public long PriceAfterDiscounts { get; private set; }
	public byte TaxPercent { get; set; }
	public long TaxPrice { get; private set; }
	public long FinalPriceOfRow { get; private set; }
	public required int InvoiceNumber { get; set; }
	public required int InvoiceUserId { get; set; }
	
	[ForeignKey("InvoiceNumber, InvoiceUserId")]
	public Invoice? Invoice { get; set; }

	public void CalculatePrivateFields()
	{
		PriceForAll = PriceForEach * Quantity;
		PriceAfterDiscounts = PriceForAll - Discount;
		TaxPrice = (PriceAfterDiscounts * TaxPercent) / 100;
		FinalPriceOfRow = PriceAfterDiscounts + TaxPrice;
	}
}