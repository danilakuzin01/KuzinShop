using KuzinShop.Models;
using Microsoft.EntityFrameworkCore;

namespace KuzinShop.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        //public User GetByLogin(string login)
        //{
        //    return _context.Users.Where(u=> u.UserName.Equals(login));
        //}

    }
}
