namespace MyServices;

public class InvoiceDeleteVM : IValidatableObject
{
	[Display(Name = "شماره فاکتور")]
	public int InvoiceNumber { get; set; }

	public InvoiceType? InvoiceType { get; set; }

	public long? FinalPrice { get; set; }

	public CurrencyType? CurrencyType { get; set; }

	[Display(Name = "متن درخواستی")]
	[Required(ErrorMessage = "وارد نمودن {0} الزامی می‌باشد.")]
	public string DeleteText { get; set; } = default!;

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		if (!DeleteText.CleanString().Equals("پاک شود".CleanString()))
			yield return new ValidationResult("واژه‌ی درخواستی به درستی وارد نشده است.", [nameof(DeleteText)]);
	}
}
