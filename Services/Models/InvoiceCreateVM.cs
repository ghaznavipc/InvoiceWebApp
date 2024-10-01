namespace MyServices;

public class InvoiceCreateVM
{
	[Display(Name = "شماره فاکتور")]
	[Required(ErrorMessage = "وارد نمودن {0} الزامی می‌باشد.")]
	public int InvoiceNumber { get; set; }

	[Display(Name = "مشتری")]
	public string? CustomerFullName { get; set; }

	public IEnumerable<string>? CustomerFullNames { get; set; }
	public IEnumerable<string>? ProductOrServiceTitles { get; set; }

	[Display(Name = "اصلاحی از")]
	public int? PreviousInvoiceNumber { get; set; }

	[Display(Name = "نوع فاکتور")]
	[Required(ErrorMessage = "انتخاب {0} الزامی می‌باشد.")]
	public InvoiceType InvoiceType { get; set; }

	[Display(Name = "ردیف‌ها")]
	[Required(ErrorMessage = "وارد نمودن {0} الزامی می‌باشد.")]
	public required IList<RowVM> RowVMs { get; set; }

	[Display(Name = "میزان بدهی یا طلب")]
	[Required(ErrorMessage = "وارد نمودن {0} الزامی می‌باشد.")]
	public long Debit { get; set; }

	[Display(Name = "پیش‌پرداخت")]
	[Required(ErrorMessage = "وارد نمودن {0} الزامی می‌باشد.")]
	public long Prepayment { get; set; }

	[Display(Name = "نوع ارز")]
	[Required(ErrorMessage = "انتخاب {0} الزامی می‌باشد.")]
	public CurrencyType CurrencyType { get; set; }

	[Display(Name = "توضیح")]
	public string? Description { get; set; }

	[Display(Name = "از پیش‌نویس خارج و ثبت نهایی شود.")]
	public bool WantToSubmit { get; set; }

}
