using System.Threading.Tasks;

namespace WebFramework.Configuration;

public static class RolesInitializer
{
	public static async Task Initialize(UserManager<User> userManager, RoleManager<Role> roleManager)
	{
		string[] roleNames = { UserRole.Owner, UserRole.Admin, UserRole.VerifiedUser, UserRole.BaseUser };

		foreach (string roleName in roleNames)
		{
			var roleExist = await roleManager.RoleExistsAsync(roleName);

			if (!roleExist)
				await roleManager.CreateAsync(new Role(roleName));
		}

		User? owner = await userManager.FindByEmailAsync("ghaznavipc@gmail.com");

		if (owner == null)
		{
			owner = new()
			{
				UserName = "ghaznavipc@gmail.com",
				FullName= "ghaznavipc",
				Email = "ghaznavipc@gmail.com",
				PhoneNumber = "09335262646",
				PersonType = PersonType.Actual,
				EmailConfirmed = true,
				PhoneNumberConfirmed = true,
			};

			await userManager.CreateAsync(owner, "gh8zn8V!");
			await userManager.AddToRoleAsync(owner, UserRole.Owner);
		}
	}
}