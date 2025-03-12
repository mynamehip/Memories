using DataAccess;
using UserService.Models;

namespace UserService.Data
{
    public class UserRepository : BaseRepository<UserInfo, Guid, ApplicationDbContext>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }
    }
}
