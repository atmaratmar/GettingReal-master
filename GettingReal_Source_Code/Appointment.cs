using System;

namespace GettingReal_Source_Code
{
    public class Appointment
    {
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Notes { get; set; }
        public Customer Customer { get; set; }

        public Appointment()
        {

        }
        public Appointment(DateTime date, string startTime, string endTime, string notes, Customer customer)
        {
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Notes = notes;
            Customer = customer;
        }

        public static string ChangeNotes(string notes)
        {
            string holdingSentence = "";
            string[] sentences = notes.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string sentence in sentences)
            {
                string trimSentence = sentence.Trim();
                string firstLetter = trimSentence.Substring(0, 1);
                firstLetter = firstLetter.ToUpper();
                holdingSentence = holdingSentence + " " + firstLetter + trimSentence.Substring(1).ToLower() + '.';
            }
            return holdingSentence.Trim();
        }
    }
}
