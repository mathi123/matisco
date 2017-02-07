using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Matisco.Domain.Specs
{
    [TestClass]
    public class ValidationProviderSpecs
    {
        private TestCustomerValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            var provider = new TestCustomerValidationProvider(null);
            provider.DefineRules();
            _validator = new TestCustomerValidator(provider);
        }

        [TestMethod]
        public void TestDefineRule()
        {
            var customer = new TestCustomer()
            {
                Email = "emailwithoutAt.com",
                City = "Gent",
                Country = "Belgium",
                Name = "Jan Met De Pet"
            };
            _validator.Customer = customer;

            Assert.IsFalse(_validator.IsValid());
            customer.Email = "Email@email.com";
            var isValid = _validator.IsValid();
            Assert.IsTrue(isValid);
        }
    }
}
