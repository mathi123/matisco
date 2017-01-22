using System.Collections.Generic;

namespace Matisco.SomeApplication.CustomerManagement
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();

        Customer GetById(int id);

        bool Save(Customer customer);
    }
}
