using System;
using System.Collections.Generic;

namespace GettingReal_Source_Code
{
    public class Customer
    {
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public Dictionary<string, Customer> ListOfCustomers { get; set; }
        public Customer()
        {

        }
        public Customer(string firstName, string lastName, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
        }
        public static string ChangeName(string name)
        {
            string holdingWord = "";
            string[] words = name.Split(' ');
            foreach (string word in words)
            {
                string firstLetter = word.Substring(0, 1);
                firstLetter = firstLetter.ToUpper();
                holdingWord = holdingWord + " " + firstLetter + word.Substring(1).ToLower();
            }
            return holdingWord.Trim();
        }
        public static string SplitPhoneNumber(string phone)
        {
            phone = phone.Replace(" ", String.Empty);
            string number = "";
            if (CheckPhoneNumberFormat(phone) == true)
            {
                number = phone.Insert(6, " ");
                number = number.Insert(4, " ");
                number = number.Insert(2, " ");
            }
            return number;
        }
        public static bool CheckPhoneNumberFormat(string phone)
        {
            bool IsEnoughLength = true;
            if (phone.Replace(" ", String.Empty).Length != 8)
            {
                IsEnoughLength = false;
            }
            return IsEnoughLength;
        }

        public static bool CheckPhoneNumberForSomethingDifferentThanDigits(string phone)
        {
            List<char> numbers = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            bool IsOnlyInts = true;
            foreach (char digit in phone)
            {
                if (numbers.Contains(digit) == false)
                {
                    IsOnlyInts = false;
                }
            }
            return IsOnlyInts;
        }
        public static bool CheckName(string name)
        {
            List<char> letters = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'æ', 'ø', 'å', ' ' };
            bool IsOnlyLetters = true;
            foreach (char character in name)
            {
                if (letters.Contains(character) == false)
                {
                    IsOnlyLetters = false;
                }
            }
            return IsOnlyLetters;
        }
    }
}
