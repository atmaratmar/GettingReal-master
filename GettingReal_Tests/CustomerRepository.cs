using GettingReal_Source_Code;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GettingReal_Tests
{
    [TestClass]
    public class CustomerRepositoryTests
    {
        private CustomerRepository CreateRepositoryCu()
        {
            return new CustomerRepository();
        }

        [TestMethod]
        public void ClearRepo()
        {
            CustomerRepository repoCu = CreateRepositoryCu();
            repoCu.Clear();

            Assert.AreEqual(0, repoCu.CountCustomers());

            Customer john = repoCu.Create("John", "Smith", "98 09 89 76");

            Assert.AreEqual(1, repoCu.CountCustomers());

            repoCu.Clear();
            Assert.AreEqual(0, repoCu.CountCustomers());

        }
        [TestMethod]
        public void CreateCustomer()
        {
            CustomerRepository repoCu = CreateRepositoryCu();
            repoCu.Clear();

            Assert.AreEqual(0, repoCu.CountCustomers());

            Customer john = repoCu.Create("John", "Smith", "98 09 89 76");

            Assert.AreEqual(1, repoCu.CountCustomers());
        }

        [TestMethod]
        public void LoadCustomer()
        {
            CustomerRepository repoCu = CreateRepositoryCu();
            repoCu.Clear();

            Assert.AreEqual(0, repoCu.CountCustomers());

            Customer ben = repoCu.Create("Ben", "Mortensen", "11 23 65 47");

            Customer loadedCustomer = repoCu.Load("11 23 65 47");
            Assert.AreEqual("Ben" + " " + "Mortensen", ben.FirstName + " " + ben.LastName);
        }

        [TestMethod]
        public void RemoveCustomer()
        {
            CustomerRepository repoCu = CreateRepositoryCu();
            repoCu.Clear();

            Assert.AreEqual(0, repoCu.CountCustomers());

            Customer john = repoCu.Create("John", "Smith", "98 09 89 76");

            Assert.AreEqual(1, repoCu.CountCustomers());

            Customer ben = repoCu.Create("Ben", "Mortensen", "11 23 65 47");

            Assert.AreEqual(2, repoCu.CountCustomers());

            repoCu.Delete("11 23 65 47");

            Assert.AreEqual(1, repoCu.CountCustomers());
        }
    }
}

