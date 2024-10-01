using Microsoft.AspNetCore.Identity;

namespace Data;

public class ApplicationIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DuplicateUserName(string userName) => new() {Code = nameof(DuplicateUserName), Description = $"نام کاربری '{userName}' قبلا توسط شخص دیگری انتخاب شده است." };

    public override IdentityError InvalidUserName(string userName) => new() { Code = nameof(InvalidUserName), Description = "نام کاربری باید شامل کاراکترهای (0-9) و (a-z) باشد." };

    public override IdentityError PasswordMismatch() => new() { Code = nameof(PasswordMismatch), Description = "نام کاربری یا کلمه‌ی عبور وارد شده صحیح نمی‌باشد(؟)" };

    public override IdentityError PasswordRequiresNonAlphanumeric() => new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "کلمه عبور باید حداقل شامل یک کاراکتر غیرعددی و غیر حرفی باشد.(@,%,#,...)" };

    public override IdentityError PasswordRequiresDigit() => new() { Code = nameof(PasswordRequiresDigit), Description = "کلمه عبور باید حداقل شامل یک عدد انگلیسی باشد." };

    public override IdentityError PasswordRequiresLower() => new() { Code = nameof(PasswordRequiresLower), Description = "کلمه عبور باید حداقل شامل یک حرف کوچک انگلیسی باشد." };

    public override IdentityError PasswordRequiresUpper() => new() { Code = nameof(PasswordRequiresUpper), Description = "کلمه عبور باید حداقل شامل یک حرف بزرگ انگلیسی باشد." };

    public override IdentityError PasswordTooShort(int length) => new() { Code = nameof(PasswordTooShort), Description = $"کلمه عبور باید حداقل شامل {length} کاراکتر باشد." };

    public override IdentityError DuplicateRoleName(string role) => new() { Code = nameof(DuplicateRoleName), Description = $"نقش '{role}' تکراری است." };

    public override IdentityError DuplicateEmail(string email) => new() { Code = nameof(DuplicateEmail), Description = $"شما با ایمیل '{email}' قبلا ثبت نام کرده‌اید." };

    public override IdentityError InvalidEmail(string email) => new() { Code = nameof(InvalidEmail), Description = $".آرایش ایمیل وارد شده نامعتبر است" };

    public override IdentityError InvalidToken() => new() { Code = nameof(InvalidToken), Description = "توکن وارد شده نامعتبر است." };

    public override IdentityError DefaultError() => new() { Code = nameof(DefaultError), Description = "خطایی رخ داده است." };
}
