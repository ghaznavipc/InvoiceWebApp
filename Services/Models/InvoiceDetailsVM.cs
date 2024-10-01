namespace MyServices;

public class InvoiceDetailsVM
{
	[Display(Name = "شماره فاکتور")]
	public int InvoiceNumber { get; set; }

	[Display(Name = "نوع فاکتور")]
	public InvoiceType InvoiceType { get; set; }

	[Display(Name = "مشتری")]
	public string? CustomerFullName { get; set; }

	[Display(Name = "اصلاحی از")]
	public int? PreviousInvoiceNumber { get; set; }

	[Display(Name = "ردیف‌ها")]
	public required IList<RowVM> RowVMs { get; set; }

	[Display(Name = "مجموع بدون مالیات")]
	public long? TotalPriceWithoutTax { get; set; }

	[Display(Name = "مجموع مالیات")]
	public long? TotalOfTax { get; set; }

	[Display(Name = "مجموع")]
	public long? TotalPrice { get; set; }

	[Display(Name = "میزان بدهی یا طلب")]
	public long Debit { get; set; }

	[Display(Name = "پیش‌پرداخت")]
	public long Prepayment { get; set; }

	[Display(Name = "قیمت نهایی")]
	public long? FinalPrice { get; set; }

	[Display(Name = "نوع ارز")]
	public CurrencyType CurrencyType { get; set; }

	[Display(Name = "توضیح")]
	public string? Description { get; set; }

	[Display(Name = "تاریخ ثبت")]
	public DateTime? SubmitDate { get; set; }
}
