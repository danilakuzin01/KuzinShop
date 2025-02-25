﻿using KuzinShop.Models;
using Microsoft.EntityFrameworkCore;

namespace KuzinShop.Repositories.Impl
{
    public class UserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }
        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }
        public User Get(string id)
        {
            return _context.Users.FirstOrDefault(x => x.Id.Equals(id));
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void ChangeUserStatus(User user)
        {
            user.IsActive = !user.IsActive;
        }

        //public User GetByLogin(string login)
        //{
        //    return _context.Users.Where(u=> u.UserName.Equals(login));
        //}

    }
}
