using Core.Models;
using Core.Repositories;

namespace Data.Repositories
{
    public class UserRepository : GenericRepository<User, AppUserContext>, IUserRepository<AppUserContext>
    {
        public UserRepository(AppUserContext context) : base(context)
        {
        }
    }
}