using GettingReal_Source_Code;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GettingReal_Tests
{
    [TestClass]
    public class AppointmentTest
    {
        private Appointment appointment;

        [TestInitialize]
        public void SetupForTest()
        {
            appointment = new Appointment();
        }

        [TestMethod]
        public void ShouldStartEverySentenceInNotesWithUpperCase()
        {
            appointment.Notes = "black-eyed wolf. medium size tattoo.";
            Assert.AreEqual("Black-eyed wolf. Medium size tattoo.", Appointment.ChangeNotes(appointment.Notes));
        }
    }
}
