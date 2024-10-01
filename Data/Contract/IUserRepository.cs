using Common;

namespace Data.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken);

    Task<SiteResult> AddAsync(User user, string password,string role, CancellationToken cancellationToken);
    Task UpdateSecuirtyStampAsync(User user, CancellationToken cancellationToken);
    Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
}