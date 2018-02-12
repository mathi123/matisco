using System;
using System.Collections.Generic;
using System.Linq;
using Example.BusinessApp.Sales.Shared;

namespace Example.BusinessApp.Sales.Business
{
    public class CustomerManager : ICustomerManager
    {
        private readonly SalesDbContext _context;

        public CustomerManager(SalesDbContext context)
        {
            _context = context;
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public void UpdateCustomer(Customer customer)
        {
            
        }
    }
}
