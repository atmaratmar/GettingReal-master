using System;
using System.Collections.Generic;
using GettingReal_Source_Code;
using System.Data.SqlClient;
using System.Data;

namespace GettingReal
{
    public class UI
    {
        public CustomerRepository cuRepo = new CustomerRepository();
        public AppointmentRepository appointRepo = new AppointmentRepository();
        string openingHour = "11:00";
        string closingHour = "18:00";
        private static string connectionString = "Server=ealdb1.eal.local;Database=EJL67_DB;User ID=ejl67_usr;Password=Baz1nga67";

        static void Main(string[] args)
        {
            UI myProgram = new UI();
            myProgram.Run();
        }
        public void Run()
        {
            RefreshCuRepository();
            RefreshAppRepository();
            Menu();
        }
        private int ChooseACommand()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("1) Insert new customer");
            Console.WriteLine("2) Show all customers");
            Console.WriteLine("3) Search customers by phone number");
            Console.WriteLine("4) Make new appointment");
            Console.WriteLine("5) Show all appointments");
            Console.WriteLine("6) Show available times");
            Console.WriteLine("7) Change the appointment");
            Console.WriteLine("8) Show all appointments of a customer");
            Console.WriteLine("9) Delete an appointment of a customer");
            Console.WriteLine("10) Show all appointments for a specific day");
            Console.WriteLine("11) End program");
            Console.WriteLine();
            Console.WriteLine("Please choose a command:");

            int inputNum;
            if (Int32.TryParse(Console.ReadLine(), out inputNum) == false)
            {
                inputNum = 0;
            }
            Console.Clear();
            return inputNum;
        }
        public void Menu()
        {
            bool isRunning = true;
            do
            {
                int input = ChooseACommand();
                switch (input)
                {
                    case 1:
                        InsertCustomer();
                        break;
                    case 2:
                        ShowAllCustomers();
                        break;
                    case 3:
                        SearchCustomerByPhone();
                        break;
                    case 4:
                        MakeNewAppointment();
                        break;
                    case 5:
                        ShowAllAppointments();
                        break;
                    case 6:
                        ShowAvailableTimeForThatDay();
                        break;
                    case 7:
                        ChangeAppointmentDateTime();
                        break;
                    case 8:
                        SearchAppointmentByCustomerPhone();
                        break;
                    case 9:
                        DeleteAppointment();
                        break;
                    case 10:
                        ShowAppointmentsForASpecificDay();
                        break;
                    case 11:
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Wrong input! Try again!");
                        Console.ReadKey();
                        break;
                }
                Console.Clear();
            } while (isRunning);
        }

        public void InsertCustomer()
        {
            Customer customer = new Customer();
            DBcontroler DB = new DBcontroler();
            string firstName = InputCustomer("first name");
            Console.Clear();
            string lastName = InputCustomer("last name");
            Console.Clear();
            string phone = InputCustomer("telephone number");

            customer.FirstName = Customer.ChangeName(firstName);
            customer.LastName = Customer.ChangeName(lastName);
            customer.Phone = Customer.SplitPhoneNumber(phone);

            DB.InsertCustomer(customer);
            Console.WriteLine("The customer was added to the system!");
            Console.ReadKey();
        }

        public void MakeNewAppointment()
        {
            string startTime = "";
            string endTime = "";
            bool converted = false;
            DateTime date;
            DBcontroler DB = new DBcontroler();
            do
            {
                Console.Write("Date (dd-mm-yyyy): ");
                converted = DateTime.TryParse(Console.ReadLine(), out date);
                if (converted == false)
                {
                    Console.WriteLine();
                    Console.WriteLine("Wrong input! Try again!");
                    Console.ReadKey();
                    Console.Clear();
                }
            } while (converted == false);

            ShowAvailableTime(date);
            do
            {
                Console.Write("Start time (hh:mm): ");
                startTime = Console.ReadLine();
            } while (CheckHours(startTime) == false || CheckMinutes(startTime) == false);
            do
            {
                Console.Write("End time (hh:mm): ");
                endTime = Console.ReadLine();
            } while (CheckHours(endTime) == false || CheckMinutes(endTime) == false);

            Console.Write("Notes: ");
            string notes = Appointment.ChangeNotes(Console.ReadLine());
            string phone = "";
            bool phoneIsRight = false;
            do
            {
                phoneIsRight = false;
                Console.Write("Customer's phone number: ");
                phone = Console.ReadLine();
                if (PhoneNumberChecking(phone) == true)
                {
                    phoneIsRight = true;
                    phone = Customer.SplitPhoneNumber(phone);
                }
            } while (phoneIsRight == false);

            Customer customer = cuRepo.Load(phone);
            Appointment appointment = new Appointment(date, startTime, endTime, notes, customer);
            DB.InsertAppointment(appointment);
            Console.WriteLine("The appointment was made!");
            Console.ReadKey();
        }
        public string InputCustomer(string parameter)
        {
            Customer customer = new Customer();
            bool isParOK = false;
            string input = "";
            while (isParOK == false)
            {
                string openingSentence = "Enter the " + parameter + " of the customer:";
                Console.WriteLine(openingSentence);
                input = Customer.ChangeName(Console.ReadLine());
                switch (parameter)
                {
                    case "first name":
                        isParOK = Customer.CheckName(input.ToLower());
                        break;
                    case "last name":
                        isParOK = Customer.CheckName(input.ToLower());
                        break;
                    case "telephone number":
                        isParOK = true;
                        if (Customer.CheckPhoneNumberFormat(input) == false || Customer.CheckPhoneNumberForSomethingDifferentThanDigits(input) == false)
                        {
                            isParOK = false;
                        }
                        break;
                }
                if (isParOK == false)
                {
                    Error();
                }
            }
            return input;
        }
        public void Error()
        {
            string errorMessage = "Wrong format! Please try again:";
            Console.WriteLine(errorMessage);
            Console.ReadKey();
            Console.Clear();
        }

        private void ShowAllCustomers()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmdShowAllCustomers = new SqlCommand("SP_SHOW_ALL_CUSTOMERS", con);
                cmdShowAllCustomers.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmdShowAllCustomers.ExecuteReader();
                int counter = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (counter == 5)
                        {
                            Console.WriteLine("If you want to continue press any key!");
                            Console.ReadKey();
                            Console.WriteLine();
                            counter = 0;
                        }
                        string customerFirstName = reader["FIRST_NAME"].ToString();
                        string customerLastName = reader["LAST_NAME"].ToString();
                        string customerPhoneNumber = reader["PHONE_NUMBER"].ToString();

                        ShowCustInfo(customerFirstName, customerLastName, customerPhoneNumber);
                        counter++;
                    }
                    Console.ReadKey();
                }
            }
        }

        public void RefreshCuRepository()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmdShowAllCustomers = new SqlCommand("SP_SHOW_ALL_CUSTOMERS", con);
                cmdShowAllCustomers.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmdShowAllCustomers.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string customerFirstName = reader["FIRST_NAME"].ToString();
                        string customerLastName = reader["LAST_NAME"].ToString();
                        string customerPhoneNumber = reader["PHONE_NUMBER"].ToString();
                        cuRepo.Create(customerFirstName, customerLastName, customerPhoneNumber);
                    }
                }
            }
        }
        public void RefreshAppRepository()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmdShowAllAppointments = new SqlCommand("SP_SHOW_ALL_APPOINTMENTS", con);
                cmdShowAllAppointments.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmdShowAllAppointments.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DateTime appointmentDate = Convert.ToDateTime(reader["APPOINTMENT_DATE"]);
                        string appointmentStartTime = reader["START_TIME"].ToString();
                        string appointmentEndTime = reader["END_TIME"].ToString();
                        string appointmentNotes = reader["NOTES"].ToString();
                        string customerPhone = reader["PHONE_NUMBER"].ToString();
                        appointRepo.Create(appointmentDate, appointmentStartTime, appointmentEndTime, appointmentNotes, customerPhone);
                    }
                }
            }
        }

        private void ShowAllAppointments()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmdShowAllAppointments = new SqlCommand("SP_SHOW_ALL_APPOINTMENTS", con);
                cmdShowAllAppointments.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmdShowAllAppointments.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DateTime appointmentDate = Convert.ToDateTime(reader["APPOINTMENT_DATE"]);
                        string appointmentStartTime = reader["START_TIME"].ToString();
                        string appointmentEndTime = reader["END_TIME"].ToString();
                        string appointmentNotes = reader["NOTES"].ToString();
                        string customerPhone = reader["PHONE_NUMBER"].ToString();

                        Console.WriteLine("Customer : " + cuRepo.Load(customerPhone).FirstName + " " + cuRepo.Load(customerPhone).LastName);
                        ShowAppointInfo(appointmentDate.ToString("dd-MM-yyyy"), appointmentStartTime, appointmentEndTime, appointmentNotes, customerPhone);
                    }
                    Console.ReadKey();
                }
            }
        }

        private void ShowAppointmentsForASpecificDay()
        {
            Console.Write("Date (dd-mm-yyyy): ");
            DateTime date = Convert.ToDateTime(Console.ReadLine());
            ShowAllAppointmentsForThatDay(date);
        }

        private void ShowAllAppointmentsForThatDay(DateTime date)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmdShowAppointmentsByDate = new SqlCommand("SHOW_APPOINTMENTS_BY_DATE", con);
                cmdShowAppointmentsByDate.CommandType = CommandType.StoredProcedure;

                cmdShowAppointmentsByDate.Parameters.Add(new SqlParameter("@APPOINTMENT_DATE", date));

                SqlDataReader reader = cmdShowAppointmentsByDate.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DateTime appointmentDate = Convert.ToDateTime(reader["APPOINTMENT_DATE"]);
                        string appointmentStartTime = reader["START_TIME"].ToString();
                        string appointmentEndTime = reader["END_TIME"].ToString();
                        string appointmentNotes = reader["NOTES"].ToString();
                        string customerPhone = reader["PHONE_NUMBER"].ToString();

                        Console.WriteLine("Customer : " + cuRepo.Load(customerPhone).FirstName + " " + cuRepo.Load(customerPhone).LastName);
                        ShowAppointInfo(appointmentDate.ToString("dd-MM-yyyy"), appointmentStartTime, appointmentEndTime, appointmentNotes, customerPhone);
                    }
                    Console.ReadKey();
                }
            }
        }

        private void ShowAvailableTimeForThatDay()
        {
            Console.Write("Date (dd-mm-yyyy): ");
            DateTime date = Convert.ToDateTime(Console.ReadLine());
            ShowAvailableTime(date);
            Console.ReadKey();
        }

        private void ShowAvailableTime(DateTime date)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmdShowAvailableTimeByDate = new SqlCommand("SP_SHOW_AVAILABLE_TIME_BY_DATE", con);
                cmdShowAvailableTimeByDate.CommandType = CommandType.StoredProcedure;

                cmdShowAvailableTimeByDate.Parameters.Add(new SqlParameter("@APPOINTMENT_DATE", date));

                SqlDataReader reader = cmdShowAvailableTimeByDate.ExecuteReader();
                List<Appointment> listOfAvailableTime = new List<Appointment>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string appointmentStartTime = reader["START_TIME"].ToString();
                        string appointmentEndTime = reader["END_TIME"].ToString();
                        Appointment appointment = new Appointment(date, appointmentStartTime, appointmentEndTime, "", new Customer());
                        listOfAvailableTime.Add(appointment);
                    }

                    Console.WriteLine();
                    Console.WriteLine("The available time frames you can choose from:");
                    if (Convert.ToInt32(listOfAvailableTime[0].StartTime.Substring(0, 2)) > Convert.ToInt32(openingHour.Substring(0, 2)))
                    {
                        Console.WriteLine(openingHour + " - " + listOfAvailableTime[0].StartTime);
                    }

                    for (int i = 0; i < listOfAvailableTime.Count - 1; i++)
                    {
                        Console.WriteLine(listOfAvailableTime[i].EndTime + " - " + listOfAvailableTime[i + 1].StartTime);
                    }

                    if (Convert.ToInt32(listOfAvailableTime[listOfAvailableTime.Count - 1].EndTime.Substring(0, 2)) < Convert.ToInt32(closingHour.Substring(0, 2)))
                    {
                        Console.WriteLine(listOfAvailableTime[listOfAvailableTime.Count - 1].EndTime + " - " + closingHour);
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("The day is free! Choose from 11:00 to 18:00.");
                    Console.WriteLine();
                }
            }

        }

        private void SearchCustomerByPhone()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmdShowCustomerByPhone = new SqlCommand("SHOW_CUSTOMER_BY_PHONE", con);
                cmdShowCustomerByPhone.CommandType = CommandType.StoredProcedure;

                string phoneInput = TakingPhoneNumber();

                cmdShowCustomerByPhone.Parameters.Add(new SqlParameter("@PHONE_NUMBER", Customer.SplitPhoneNumber(phoneInput)));

                SqlDataReader reader = cmdShowCustomerByPhone.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string customerFirstName = reader["FIRST_NAME"].ToString();
                        string customerLastName = reader["LAST_NAME"].ToString();
                        string customerPhoneNumber = reader["PHONE_NUMBER"].ToString();

                        ShowCustInfo(customerFirstName, customerLastName, customerPhoneNumber);
                    }
                    Console.ReadKey();
                }
            }
        }

        private void SearchAppointmentByCustomerPhone()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmdShowAppointmentByPhone = new SqlCommand("SHOW_APPOINTMENT_BY_PHONE", con);
                cmdShowAppointmentByPhone.CommandType = CommandType.StoredProcedure;

                string phoneInput = TakingPhoneNumber();
                cmdShowAppointmentByPhone.Parameters.Add(new SqlParameter("@PHONE_NUMBER", Customer.SplitPhoneNumber(phoneInput)));

                SqlDataReader reader = cmdShowAppointmentByPhone.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DateTime appointmentDate = Convert.ToDateTime(reader["APPOINTMENT_DATE"]);
                        string appointmentStartTime = reader["START_TIME"].ToString();
                        string appointmentEndTime = reader["END_TIME"].ToString();
                        string appointmentNotes = reader["NOTES"].ToString();
                        string customerPhone = reader["PHONE_NUMBER"].ToString();

                        Console.WriteLine("Customer : " + cuRepo.Load(customerPhone).FirstName + " " + cuRepo.Load(customerPhone).LastName);
                        ShowAppointInfo(appointmentDate.ToString("dd-MM-yyyy"), appointmentStartTime, appointmentEndTime, appointmentNotes, customerPhone);
                    }
                    Console.ReadKey();
                }
            }
        }

        private string TakingPhoneNumber()
        {
            string phoneInput = "";
            bool inputCorrect = true;
            do
            {
                inputCorrect = false;
                Console.Clear();

                Console.WriteLine("(Press x if you want to exit.)");
                Console.WriteLine();
                Console.Write("Customer's phone number to search for appointments: ");
                phoneInput = Console.ReadLine();
                if (phoneInput.ToLower() != "x")
                {
                    inputCorrect = PhoneNumberChecking(phoneInput);
                }
                else
                {
                    inputCorrect = true;
                }
            } while (inputCorrect == false);
            Console.Clear();
            return phoneInput;
        }

        private void ChangeAppointmentDateTime()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmdChangeAppointmentByPhone = new SqlCommand("SP_CHANGE_APPOINTMENT_DATE_AND_TIME", con);
                cmdChangeAppointmentByPhone.CommandType = CommandType.StoredProcedure;

                SearchAppointmentByCustomerPhone();

                string startTime = "";
                string endTime = "";
                string phoneNum = "";
                DateTime oldDate;
                bool converted = false;
                DateTime date;
                DBcontroler DB = new DBcontroler();
                Console.Write("Choose the date you want to change (dd-mm-yyyy): ");
                oldDate = Convert.ToDateTime(Console.ReadLine());
                do
                {
                    Console.Write("New date to set the appointment (dd-mm-yyyy): ");
                    converted = DateTime.TryParse(Console.ReadLine(), out date);
                    if (converted == false)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Wrong input! Try again!");
                        Console.ReadKey();
                        Console.Clear();
                    }
                } while (converted == false);

                ShowAvailableTime(date);
                Console.ReadKey();
                do
                {
                    Console.Write("New start time for the appointment (hh:mm): ");
                    startTime = Console.ReadLine();

                } while (CheckHours(startTime) == false || CheckMinutes(startTime) == false);
                do
                {
                    Console.Write("New end time for the appointment (hh:mm): ");
                    endTime = Console.ReadLine();
                } while (CheckHours(endTime) == false || CheckMinutes(endTime) == false);

                do
                {
                    Console.Write("Confirm customer's phone number : ");
                    phoneNum = Console.ReadLine();

                } while (PhoneNumberChecking(phoneNum) == false);

                DB.ChangeAppointment(phoneNum, date, startTime, endTime, oldDate);
                Console.ReadKey();
            }
        }


        private void DeleteAppointment()
        {
            bool converted = false;
            DateTime date;
            DBcontroler DB = new DBcontroler();

            do
            {
                Console.Write("Old date of the appointment (dd-mm-yyyy): ");
                converted = DateTime.TryParse(Console.ReadLine(), out date);
                if (converted == false)
                {
                    Console.WriteLine();
                    Console.WriteLine("Wrong input! Try again!");
                    Console.ReadKey();
                    Console.Clear();
                }
            } while (converted == false);

            string phoneInput = TakingPhoneNumber();

            DB.DeleteAppointment(phoneInput, date);
            Console.WriteLine("The appointment was deleted from the system!");
            Console.ReadKey();
        }

        public bool PhoneNumberChecking(string phone)
        {
            bool inputCorrect = true;
            if (Customer.CheckPhoneNumberFormat(phone) == false || Customer.CheckPhoneNumberForSomethingDifferentThanDigits(phone) == false)
            {
                Error();
                inputCorrect = false;
            }

            if (inputCorrect == true)
            {
                phone = Customer.SplitPhoneNumber(phone);

                if (cuRepo.Exists(phone) == false)
                {
                    inputCorrect = false;
                    Console.WriteLine("The phone number is not found in the system!");
                    Console.ReadKey();
                }
            }
            return inputCorrect;
        }

        private void ShowCustInfo(string customerFirstName, string customerLastName, string customerPhoneNumber)
        {
            Console.WriteLine("First name - " + customerFirstName);
            Console.WriteLine("Last name - " + customerLastName);
            Console.WriteLine("Telephone number - " + customerPhoneNumber);
            Console.WriteLine();
        }

        private bool CheckHours(string hour)
        {
            bool isWithinTimeFrame = true;
            int hourNumber = Convert.ToInt32(hour.Substring(0, 2));
            int openingHourNumber = Convert.ToInt32(openingHour.Substring(0, 2));
            int closingHourNumber = Convert.ToInt32(closingHour.Substring(0, 2));
            if ((hourNumber < openingHourNumber) || (hourNumber > closingHourNumber))
            {
                isWithinTimeFrame = false;
                Console.WriteLine("The chosen hour is not within the time frame! Please choose another one.");
            }
            return isWithinTimeFrame;
        }

        private bool CheckMinutes(string hour)
        {
            int minutesNumber = Convert.ToInt32(hour.Substring(3, 2));
            bool isBelowAnHour = true;
            if ((minutesNumber > 59) || (minutesNumber < 00))
            {
                isBelowAnHour = false;
                Console.WriteLine("The chosen minutes are wrong!");
            }
            return isBelowAnHour;
        }
        private void ShowAppointInfo(string appointmentDate, string appointmentStartTime, string appointmentEndTime, string appointmentNotes, string customerPhoneNumber)
        {
            Console.WriteLine("Date - " + appointmentDate);
            Console.WriteLine("Start time - " + appointmentStartTime);
            Console.WriteLine("End time - " + appointmentEndTime);
            Console.WriteLine("Notes - " + appointmentNotes);
            Console.WriteLine("Telephone number - " + customerPhoneNumber);
            Console.WriteLine();
        }
    }
}
