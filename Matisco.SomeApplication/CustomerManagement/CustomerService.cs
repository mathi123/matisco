using System.Collections.Generic;
using System.Linq;

namespace Matisco.SomeApplication.CustomerManagement
{
    public class CustomerService : ICustomerService
    {
        private readonly List<Customer> _customers = new List<Customer>()
        {
            new Customer()
            {
                Id = 1,
                Name = "Jan Met de Pet",
                Email = "jan@gmail.com"
            },
            new Customer()
            {
                Id = 2,
                Name = "Peter Met de Pet",
                Email = "jan@gmail.com"
            },
            new Customer()
            {
                Id = 3,
                Name = "Tine",
                Email = "jan@gmail.com"
            }
        };

        public IEnumerable<Customer> GetAll()
        {
            return _customers;
        }

        public Customer GetById(int id)
        {
            return _customers.SingleOrDefault(cus => cus.Id == id);
        }

        public bool Save(Customer customer)
        {
            var existing = _customers.Single(cus => cus.Id == customer.Id);
            _customers.Remove(existing);
            _customers.Add(customer);
            return true;
        }
    }
}
