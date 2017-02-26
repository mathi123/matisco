using System.Collections.Generic;
using Example.BusinessApp.Infrastructure.Models;

namespace Example.BusinessApp.Infrastructure.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();

        User GetById(int id);
    }
}
