using System.Collections.Generic;

namespace GettingReal_Source_Code
{
    public class CustomerRepository
    {
        Dictionary<string, Customer> listOfCustomers = new Dictionary<string, Customer>();
        public void Clear()
        {
            listOfCustomers.Clear();
        }
        public int CountCustomers()
        {
            int numberOfCustomers = listOfCustomers.Count;
            return numberOfCustomers;
        }
        public Customer Create(string firstName, string lastName, string phone)
        {
            Customer customer = new Customer(firstName, lastName, phone);
            listOfCustomers.Add(phone, customer);
            return customer;
        }

        public Customer Load(string phone)
        {
            if (listOfCustomers.ContainsKey(phone))
            {
                return listOfCustomers[phone];
            }
            else
            {
                return null;
            }
        }

        public void Delete(string phone)
        {
            listOfCustomers.Remove(phone);
        }

        public bool RegisterCustomer(string firstName, string lastName, string phone)
        {
            bool isRegistered = false;
            if (Exists(phone) == false)
            {
                Create(firstName, lastName, phone);
                isRegistered = true;
            }
            return isRegistered;
        }

        public bool Exists(string phone)
        {
            DBcontroler dbc = new DBcontroler();
            bool custExists = false;
            List<string> listOfPhonesFromDB = dbc.GetPhonesFromDB();
            foreach (string phoneNumber in listOfPhonesFromDB)
            {
                if (phoneNumber == phone)
                {
                    custExists = true;
                }
            }
            return custExists;
        }
    }
}
