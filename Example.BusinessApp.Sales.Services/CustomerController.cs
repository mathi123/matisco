using Example.BusinessApp.Sales.Business;
using Example.BusinessApp.Sales.Shared;
using Matisco.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Example.BusinessApp.Sales.Services
{
    [Route("api/[controller]")]

    public class CustomerController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICustomerManager _customerManager;

        public CustomerController(ILogger logger, ICustomerManager customerManager)
        {
            _logger = logger;
            _customerManager = customerManager;
        }

        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            _logger.Verbose(nameof(CustomerController), "Getting all customers");
            return _customerManager.GetAllCustomers();
        }



    }
}
