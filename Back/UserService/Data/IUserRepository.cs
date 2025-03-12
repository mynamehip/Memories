using DataAccess;
using UserService.Models;

namespace UserService.Data
{
    public interface IUserRepository : IBaseRepository<UserInfo, Guid>
    {

    }
}
