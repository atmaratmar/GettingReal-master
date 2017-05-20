using System;
using System.Collections.Generic;

namespace GettingReal_Source_Code
{
    public class AppointmentRepository
    {
        Dictionary<string, Appointment> listOfAppointments = new Dictionary<string, Appointment>();
        public void Clear()
        {
            listOfAppointments.Clear();
        }
        public int CountAppointments()
        {
            int numberOfAppointments = listOfAppointments.Count;
            return numberOfAppointments;
        }
        public Appointment Create(DateTime date, string startTime, string endTime, string notes, string phone)
        {
            CustomerRepository cuRepo = new CustomerRepository();
            Appointment appointment = new Appointment(date, startTime, endTime, notes, cuRepo.Load(phone));

            Clear();
            listOfAppointments.Add(phone, appointment);
            return appointment;
        }

        public Appointment Load(string phone)
        {
            if (listOfAppointments.ContainsKey(phone))
            {
                return listOfAppointments[phone];
            }
            else
            {
                return null;
            }
        }

        public void Delete(string phone)
        {
            listOfAppointments.Remove(phone);
        }
    }
}
