using System.Collections.Generic;
using System.Linq;
using Example.BusinessApp.Infrastructure.Models;

namespace Example.BusinessApp.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly List<Role> _roles = new List<Role>
        {
            new Role
            {
                Id = 1,
                Description = "Salesmen"
            },
            new Role
            {
                Id = 2,
                Description = "Management"
            }
        };

        public IEnumerable<Role> Get()
        {
            return _roles;
        }

        public Role GetById(int id)
        {
            return _roles.Single(role => role.Id == id);
        }

        public SaveResult Update(Role role)
        {
            var result = new SaveResult();

            try
            {

                var existingRole = GetById(role.Id);

                existingRole.Description = role.Description;
                result.Succes = true;
            }
            catch
            {
                result.ValidationErrors.Add("Role not found.");
            }

            return result;
        }

        public SaveResult Delete(int roleId)
        {
            var result = new SaveResult();

            try
            {

                var existingRole = GetById(roleId);

                _roles.Remove(existingRole);

                result.Succes = true;
            }
            catch
            {
                result.ValidationErrors.Add("Role not found.");
            }

            return result;
        }
    }
}
