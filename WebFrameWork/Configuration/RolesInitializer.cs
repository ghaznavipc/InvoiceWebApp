using System.Threading.Tasks;

namespace WebFramework.Configuration;

public static class RolesInitializer
{
	public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
	{
		string[] roleNames = { UserRole.Owner, UserRole.Admin, UserRole.VerifiedUser, UserRole.BaseUser };

		IdentityResult roleResult;

		foreach (var roleName in roleNames)
		{
			var roleExist = await roleManager.RoleExistsAsync(roleName);

			if (!roleExist)
			{
				// Create the roles and seed them to the database
				roleResult = await roleManager.CreateAsync(new Role(roleName));
			}
		}

		// Find the user with the admin email
		User user = await userManager.FindByEmailAsync("ghaznavipc@gmail.com");

		// If the admin user does not exist, create it and assign the Admin role
		if (user == null)
		{
			user = new()
			{
				UserName = "ghaznavipc@gmail.com",
				FullName= "ghaznavipc",
				Email = "ghaznavipc@gmail.com",
				PhoneNumber = "09335262646",
				PersonType = PersonType.Actual,
				EmailConfirmed = true,
				PhoneNumberConfirmed = true,
			};

			await userManager.CreateAsync(user, "gh8zn8V!");
			await userManager.AddToRoleAsync(user, UserRole.Owner);
		}
	}
}