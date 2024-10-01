
namespace MyServices;

public class SignInVM
{
	[Display(Name ="نام کاربری")]
	[Required(ErrorMessage ="وارد نمودن {0} الزامی می‌باشد.")]
	public required string UserName { get; set; }

	[Display(Name = "کلمه عبور")]
	[Required(ErrorMessage = "وارد نمودن {0} الزامی می‌باشد.")]
	public required string Password { get; set; }

	[Display(Name = "مرا به خاطر بسپار")]
	public bool RememberMe { get; set; }
}

