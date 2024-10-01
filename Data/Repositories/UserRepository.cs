using Common;
using Microsoft.AspNetCore.Identity;

namespace Data.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<Role> roleManager;
    private readonly SignInManager<User> signInManager;

    public UserRepository(ApplicationDbContext dbContext,
        UserManager<User> _userManager,
        RoleManager<Role> _roleManager,
        SignInManager<User> _signInManager)
        : base(dbContext)
    {
        userManager = _userManager;
        roleManager = _roleManager;
        signInManager = _signInManager;
    }

    public Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken)
    {
        var passwordHash = SecurityHelper.GetSha256Hash(password);
        return Table.SingleOrDefaultAsync(p => p.UserName == username && p.PasswordHash == passwordHash, cancellationToken);
    }

    public Task UpdateSecuirtyStampAsync(User user, CancellationToken cancellationToken)
    {
        
        //user.SecurityStamp = Guid.NewGuid();
        return UpdateAsync(user, cancellationToken);
    }

    //public override void Update(User entity, bool saveNow = true)
    //{
    //    entity.SecurityStamp = Guid.NewGuid();
    //    base.Update(entity, saveNow);
    //}

    public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
    {
        user.LastLoginDate = DateTimeOffset.Now;
        return UpdateAsync(user, cancellationToken);
    }

    public async Task<SiteResult> AddAsync(User user, string password,string role ,  CancellationToken cancellationToken)
    {
        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            var result2 = await userManager.AddToRoleAsync(user, role);

            return new SiteResult(true, "حساب کاربری شما با موفقیت انجام شد.");
        }
        else
        {
            var errorMessages = result.Errors.ToList().Select(p => p.Code).Distinct();
            return new SiteResult(false , errorMessages.FirstOrDefault());

        }
    }

    //public T
}
