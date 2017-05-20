using GettingReal_Source_Code;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GettingReal_Tests
{
    [TestClass]
    public class CustomerTests
    {
        private Customer customer;
        private CustomerRepository cuRepo;

        [TestInitialize]
        public void SetupForTest()
        {
            customer = new Customer();
            cuRepo = new CustomerRepository();
        }

        [TestMethod]
        public void ShouldCapitalizeFirstLetterInTheName()
        {
            customer.FirstName = "nina";
            Assert.AreEqual("Nina", Customer.ChangeName(customer.FirstName));
        }

        [TestMethod]
        public void ShouldCapitalizeEveryWord()
        {
            customer.FirstName = "anna maria";
            Assert.AreEqual("Anna Maria", Customer.ChangeName(customer.FirstName));
        }

        [TestMethod]
        public void ShouldCapitalizeFirstLetterButKeepTheRestLower()
        {
            customer.FirstName = "isAbella";
            Assert.AreEqual("Isabella", Customer.ChangeName(customer.FirstName));
        }

        [TestMethod]
        public void ShouldCapitalizeFirstLetterButKeepTheRestLowerOnEveryWord()
        {
            customer.LastName = "fLOyd gREen";
            Assert.AreEqual("Floyd Green", Customer.ChangeName(customer.LastName));
        }


        [TestMethod]
        public void ShouldSeparateEachPairOfNumbers()
        {
            customer.Phone = "22340942";
            Assert.AreEqual("22 34 09 42", Customer.SplitPhoneNumber(customer.Phone));
        }

        [TestMethod]
        public void PhoneNumberHasWrongFormat()
        {
            customer.Phone = "29870";
            Assert.IsFalse(Customer.CheckPhoneNumberFormat(customer.Phone));
        }

        [TestMethod]
        public void PhoneNumberHasGoodFormat()
        {
            customer.Phone = "74980225";
            Assert.IsTrue(Customer.CheckPhoneNumberFormat(customer.Phone));
        }

        [TestMethod]
        public void ShouldDeleteWhiteSpacesFromTheImput()
        {
            customer.Phone = "223 4094 2";
            Assert.AreEqual("22 34 09 42", Customer.SplitPhoneNumber(customer.Phone));
        }

        [TestMethod]
        public void ShouldNotInsertACustomerIfThePhoneIsTheSame()
        {
            customer.Phone = "12 34 56 78";
            Assert.IsTrue(cuRepo.Exists(customer.Phone));
        }

        [TestMethod]
        public void ShouldInsertACustomerIfThePhoneIsDifferent()
        {
            customer.Phone = "44 44 44 44";
            Assert.IsFalse(cuRepo.Exists(customer.Phone));
        }

        [TestMethod]
        public void ShouldInsertACustomer()
        {
            customer.Phone = "00 00 00 08";
            if (cuRepo.Exists(customer.Phone) == false)
            {
                Assert.IsTrue(cuRepo.RegisterCustomer("Frederik", "Loan", customer.Phone));
            }
        }
    }
}
