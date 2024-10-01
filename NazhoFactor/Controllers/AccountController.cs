using Microsoft.AspNetCore.Identity.UI.Services;
using NazhoFactor.Controllers;
using System.Net.Mail;

namespace NewsWebsite.Controllers;

public class AccountController : Controller
{
	//private readonly IEmailSender _emailSender;
	private readonly UserManager<User> _userManager;
	private readonly SignInManager<User> _signInManager;
	private readonly RoleManager<Role> _roleManager;

	public AccountController(
		//IEmailSender emailSender,
		UserManager<User> UserManager,
		SignInManager<User> signInManager,
		RoleManager<Role> roleManager
		)
	{
		//_emailSender = emailSender;
		_userManager = UserManager;
		_signInManager = signInManager;
		_roleManager = roleManager;
	}


	[HttpGet]
	public async Task<IActionResult> Index()
	{
		int userId = User.Identity!.GetUserId<int>();
		var user = await _userManager.FindByIdAsync(userId.ToString());

		return user is null ? RedirectToAction(nameof(Login)) : View(user);
	}

	[AllowAnonymous]
	[HttpGet]
	public IActionResult Login()
	{
		return View();
	}

	[AllowAnonymous]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> LoginAsync(SignInVM viewModel, CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
			return View();

		var result = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, viewModel.RememberMe, true);
		
		if (result.Succeeded)
			return RedirectToAction(nameof(InvoiceController.List), "Invoice");

		if (result.RequiresTwoFactor)
		{
			return RedirectToAction("TwoFactorAuth");
			//_logger.LogWarning($"The user attempts to login with the IP address({_accessor.HttpContext?.Connection?.RemoteIpAddress.ToString()}) and username ({viewModel.UserName}) and password ({viewModel.Password}).");
		}

		if (result.IsLockedOut)
			ModelState.AddModelError(string.Empty, "حساب کاربری شما به مدت 20 دقیقه به دلیل تلاش های ناموفق قفل شد.");
		else 
			ModelState.AddModelError(string.Empty, "نام کاربری یا کلمه عبور شما صحیح نمی‌باشد.");


		return View();
	}


	[AllowAnonymous]
	[HttpGet]
	public IActionResult Register()
	{
		return View();
	}

	[AllowAnonymous]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Register(RegisterVM vM)
	{
		if (!ModelState.IsValid)
			return View(vM);

		// Check if the provided identifier is an email or phone number
		var userExist = _userManager.Users.Any(u => u.UserName == vM.PhoneOrEmail || u.Email == vM.PhoneOrEmail || u.PhoneNumber == vM.PhoneOrEmail);
		if (userExist)
		{
			ModelState.AddModelError(string.Empty, "این نام کاربری قبلا ثبت شده است.");
			return View(vM);
		}

		var user = new User { UserName = vM.PhoneOrEmail };

		var isEmail = vM.PhoneOrEmail.Contains('@');
		if (isEmail)
			user.Email = vM.PhoneOrEmail;
		else
			user.PhoneNumber = vM.PhoneOrEmail;

		var result = await _userManager.CreateAsync(user, vM.Password);
		if (!result.Succeeded)
			return View(vM);
		
		var registeredUser = await _userManager.FindByNameAsync(user.UserName);
		if (registeredUser is null)
		{
			ModelState.AddModelError(string.Empty, "کاربر ساخته شد ولی در پیدا کردن آن خطایی به وجود آمد");
			return View(vM);
		}

		result = await _userManager.AddToRoleAsync(registeredUser, UserRole.BaseUser);
		if (!result.Succeeded)
			return View(vM);

		await _signInManager.SignInAsync(registeredUser, false);
		return RedirectToAction(nameof(InvoiceController.List), "Invoice");

#pragma warning disable CS0162 // Unreachable code detected
		if (isEmail)
		{
			string emailMessage = await GenerateEmailMessage(user);

			//await _emailSender.SendEmailAsync(user.Email, "تأیید ایمیل", emailMessage);

			return RedirectToAction(nameof(ConfirmEmail), emailMessage);
		}
		else
		{
			int verificationCode = new Random().Next(100000, 999999);

			// Save the verification code to a secure location (e.g., database)
			// In a real application, you would associate the code with the user and store it securely.
			
			// For demonstration purposes, let's store it in TempData
			TempData["VerificationCode"] = verificationCode;

			//SMSSender sender = new SMSSender();
			//sender.SendAsync()

			// Redirect the user to a page to enter the verification code
			return RedirectToAction("VerifyPhoneNumber", new { userId = user.Id });
		}
#pragma warning restore CS0162 // Unreachable code detected

		async Task<string> GenerateEmailMessage(User user)
		{
			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token }, Request.Scheme);
			var emailMessage = $"برای تأیید ایمیل خود بر روی لینک زیر کلیک کنید: {confirmationLink}";
			return emailMessage;
		}
	}


	public IActionResult ConfirmEmail(string emailMessage)
	{
		return View(model: emailMessage);
	}

	[HttpPost]
	public async Task<IActionResult> ConfirmEmail(string userId, string code)
	{

		if (userId == null || code == null)
			return RedirectToAction("Index", "Home");

		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
			return NotFound($"کاربری با شناسه‌ی '{userId}' یافت نشد.");

		var result = await _userManager.ConfirmEmailAsync(user, code);
		if (!result.Succeeded)
			throw new InvalidOperationException($"Error Confirming email for user with ID '{userId}'");

		return View();
	}


	public IActionResult VerifyPhoneNumber(string userId)
	{
		// Retrieve the verification code from TempData (in a real app, you'd retrieve it from a secure location)
		int verificationCode = TempData["VerificationCode"] as int? ?? 0;

		// For demonstration purposes, let's pass the verification code to the view
		ViewBag.VerificationCode = verificationCode;

		// In a real application, you'd typically load a view with a form for the user to enter the code.
		// Here, we're just demonstrating the process.

		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> VerifyPhoneNumber(string userId, int verificationCode)
	{
		// Retrieve the stored verification code (in a real app, you'd retrieve it from a secure location)
		int storedVerificationCode = TempData["VerificationCode"] as int? ?? 0;

		if (verificationCode == storedVerificationCode)
		{
			// Update the user's phone number confirmation status
			var user = await _userManager.FindByIdAsync(userId);
			if (user != null)
			{
				user.PhoneNumberConfirmed = true;
				await _userManager.UpdateAsync(user);
			}

			// Redirect the user to a success page or perform other actions

			return RedirectToAction(nameof(PhoneNumberVerified));
		}
		else
		{
			// Handle incorrect verification code
			ModelState.AddModelError("VerificationCode", "کد تأییدی که وارد نمودید اشتباه می‌باشد. لطفا مجددا تلاش بفرمایید.");

			// In a real application, you might redirect the user back to the verification page with an error message.
			// Here, for demonstration purposes, we redirect to the home page.
			return RedirectToAction("Index", "Home");
		}
	}

	public IActionResult PhoneNumberVerified()
	{
		return View();
	}



	[HttpGet]
	public JsonResult CheckUniqueUsername(string username)
	{
		var isUnique = !_userManager.Users.Any(u => u.UserName == username);
		return Json(isUnique);
	}

	[AllowAnonymous]
	[HttpGet]
	public JsonResult CheckUniquePhoneEmail(string phoneOrEmail)
	{
		var isUnique = !_userManager.Users.Any(u => u.UserName == phoneOrEmail || u.Email == phoneOrEmail || u.PhoneNumber == phoneOrEmail);
		return Json(isUnique);
	}


	public async Task<IActionResult> SignOutAsync()
	{
		await _signInManager.SignOutAsync();
		return RedirectToAction("Index", "Home");
	}

}