using DataAccess;
using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
    public class UserRepository : BaseRepository<UserInfo, Guid, ApplicationDbContext>, IUserRepository
    {
        ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
