using Example.BusinessApp.Sales.Shared;
using System.Collections.Generic;

namespace Example.BusinessApp.Sales.Business
{
    public interface ICustomerManager
    {
        List<Customer> GetAllCustomers();

        void UpdateCustomer(Customer customer);
    }
}