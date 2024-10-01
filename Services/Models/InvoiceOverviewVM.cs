namespace MyServices;

public class InvoiceOverviewVM
{
	[Display(Name = "شماره فاکتور")]
	public int InvoiceNumber { get; set; }

	[Display(Name = "نوع فاکتور")]
	public InvoiceType InvoiceType { get; set; }

	[Display(Name = "مشتری")]
	public string? CustomerFullName { get; set; }

	[Display(Name = "قیمت نهایی")]
	public long FinalPrice { get; set; }

	[Display(Name = "نوع ارز")]
	public CurrencyType CurrencyType { get; set; }

	[Display(Name = "تاریخ ثبت")]
	public DateTime? SubmitDate { get; set; }

	[Display(Name = "ارجاعی از")]
	public int? PreviousInvoiceNumber { get; set; }
}
