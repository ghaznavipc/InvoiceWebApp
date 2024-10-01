using Microsoft.Extensions.DependencyInjection;
using Data;

namespace WebFramework.Configuration;

public static class IdentityConfigurationExtensions
{
	public static void AddCustomIdentity(this IServiceCollection services, IdentitySettings settings)
	{
		services.AddIdentity<User, Role>(identityOptions =>
		{
		
			//Password Settings
			identityOptions.Password.RequireDigit = settings.PasswordRequireDigit;
			identityOptions.Password.RequiredLength = settings.PasswordRequiredLength;
			identityOptions.Password.RequireNonAlphanumeric = settings.PasswordRequireNonAlphanumic; //#@!
			identityOptions.Password.RequireUppercase = settings.PasswordRequireUppercase;
			identityOptions.Password.RequireLowercase = settings.PasswordRequireLowercase;

			//UserName Settings
			identityOptions.User.RequireUniqueEmail = settings.RequireUniqueEmail;


			//Singin Settings
			//identityOptions.SignIn.RequireConfirmedEmail = true;
			//identityOptions.SignIn.RequireConfirmedPhoneNumber = true;
			identityOptions.SignIn.RequireConfirmedAccount = false;

			//Lockout Settings
			//identityOptions.Lockout.MaxFailedAccessAttempts = 5;
			//identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
			//identityOptions.Lockout.AllowedForNewUsers = false;

		})
		.AddEntityFrameworkStores<ApplicationDbContext>()
		.AddErrorDescriber<ApplicationIdentityErrorDescriber>()
		.AddDefaultTokenProviders();
	}
}
