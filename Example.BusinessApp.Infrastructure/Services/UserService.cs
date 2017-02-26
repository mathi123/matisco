using System.Collections.Generic;
using System.Linq;
using Example.BusinessApp.Infrastructure.Models;

namespace Example.BusinessApp.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new List<User>()
        {
            new User()
            {
                Id = 1,
                Email = "info@matisco.be",
                Name = "main user"
            },
            new User()
            {
                Id = 2,
                Email = "info@prism.com",
                Name = "prism user"
            }
        };

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.SingleOrDefault(usr => usr.Id == id);
        }
    }
}