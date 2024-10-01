namespace MyServices;

public class RegisterVM
{
	[Display(Name = "شماره تلفن یا ایمیل")]
	[Required(ErrorMessage = "{0} الزامی است.")]
	[RegularExpression(@"(\w+@\w+\.\w{2,7})|09\d{9}", ErrorMessage = "مقدار وارد شده معتبر نمی‌باشد.")]
	public required string PhoneOrEmail { get; set; }

	[Display(Name = "کلمه عبور")]
	[Required(ErrorMessage = "وارد نمودن {0} الزامی می‌باشد.")]
	public required string Password { get; set; }

	[Display(Name = "تکرار کلمه عبور")]
	[Required(ErrorMessage = "وارد نمودن {0} الزامی می‌باشد.")]
	[Compare(nameof(Password), ErrorMessage = "کلمه عبور و تکرارش باید باهم برابر باشد.")]
	public required string ConfirmPassword { get; set; }

	public IEnumerable<ValidationResult> Validate(/*ValidationContext validationContext*/)
	{
		if (PhoneOrEmail.Equals("test@test.com", StringComparison.OrdinalIgnoreCase))
			yield return new ValidationResult("نام کاربری نمیتواند test@test.com باشد", [nameof(PhoneOrEmail)]);
		if (Password.Equals("123456"))
			yield return new ValidationResult("رمز عبور نمیتواند 123456 باشد", [nameof(Password)]);
	}
}
