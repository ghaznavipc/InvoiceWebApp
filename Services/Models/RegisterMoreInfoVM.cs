namespace MyServices;

public class RegisterMoreInfoVM
{
	[Display(Name = "نام شخص")]
	[Required(ErrorMessage = "وارد نمودن {0} الزامی می‌باشد.")]
	public required string FullName { get; set; }

	[Display(Name = "نوع شخص")]
	[Required(ErrorMessage = "انتخاب {0} الزامی می‌باشد.")]
	public required PersonType PersonType { get; set; }

	[Display(Name = "کد اقتصادی")]
	public string? EconomicCode { get; set; }

	[Display(Name = "آدرس")]
	public string? Address { get; set; }

	[Display(Name = "مهر ویا امضا")]
	public string? SignAndOrSeal { get; set; }

	[Display(Name = "نشانه بصری (لوگو)")]
	public string? Logo { get; set; }
}