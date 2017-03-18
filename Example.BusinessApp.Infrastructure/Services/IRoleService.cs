using System.Collections.Generic;
using Example.BusinessApp.Infrastructure.Models;

namespace Example.BusinessApp.Infrastructure.Services
{
    public interface IRoleService
    {
        IEnumerable<Role> Get();

        Role GetById(int id);

        SaveResult Update(Role role);

        SaveResult Delete(int roleId);
    }
}