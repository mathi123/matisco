using System.Collections.Generic;
using Example.BusinessApp.Infrastructure.Models;

namespace Example.BusinessApp.Sales.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();

        Customer GetById(int id);

        bool Save(Customer customer);
    }
}
