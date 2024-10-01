namespace MyServices;

public class RowVM
{
	[Display(Name = "نام محصول یا خدمت")]
	public string? Title { get; set; }

	[Display(Name = "قیمت هر واحد")]
	public long? PriceForEach { get; set; }

	[Display(Name = "تعداد")]
	public int? Quantity { get; set; }

	[Display(Name = "قیمت کل")]
	public long? PriceForAll { get; private set; }

	[Display(Name = "مبلغ تخفیف")]
	public long? Discount { get; set; }

	[Display(Name = "قیمت بعد تخفیف")]
	public long? PriceAfterDiscounts { get; private set; }

	[Display(Name = "درصد مالیات")]
	public byte? TaxPercent { get; set; }

	[Display(Name = "قیمت مالیات")]
	public long? TaxPrice { get; private set; }

	[Display(Name = "قیمت نهایی ردیف")]
	public long? FinalPriceOfRow { get; private set; }

	public void CalculatePrivateFields()
	{
		PriceForAll = PriceForEach * Quantity;
		PriceAfterDiscounts = PriceForAll - Discount;
		TaxPrice = (PriceAfterDiscounts * TaxPercent) / 100 ?? 0;
		FinalPriceOfRow = PriceAfterDiscounts + TaxPrice;
	}
}
