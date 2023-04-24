using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SendGrid;
using SendGrid.Helpers.Mail.Model;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using HAGSJP.WeCasa.Models;

namespace HAGSJP.WeCasa.Reminders.Test
{
    [TestClass]
    public class RemindersIntergrationTest
    {
        private NotificationService _notificationService;
        private RemindersDAO remindersDAO;
        private GroupModel _testGroup;
        [TestInitialize]
        public void Initialize()
        {
            _testGroup = new GroupModel()
            {
                GroupName = "Test Group",
                Owner = "wecasacorp@gmail.com",
                Features = new List<string> { "all" },
                Icon = "#668D6A"
            };
            var logger = new Logger(new AccountMariaDAO());
            _notificationService = new NotificationService();
            remindersDAO = new RemindersDAO();

        }

        [TestMethod]
        public async Task SendGridTest()
        {
            Console.WriteLine("P Sending before email...");
            string from = "wecasacorporation@gmail.com";
            DateTime currentTime = DateTime.Now;
            List<string> to = new List<string>() { "hvkn19@gmail.com", "bly19vy@gmail.com" };
            string reminderOption = "immediately";
            string eventType = " a new bill was added.";
            string subject = "Reminder for" + eventType;
            string message = $"This is a reminder for" + eventType;


            await NotificationService.ScheduleReminderEmail(from, to, subject, message, reminderOption, eventType);
            Console.WriteLine("Email sent..." + currentTime.ToString());
        }

        [TestMethod]
        public async Task ReminderDaoTest()
        {
            // Arrange
            var dao = new RemindersDAO();
            var groupModel = new GroupModel
            {
                GroupId = 1
            };

            // Act
            var result = dao.GetGroupEmail(groupModel);
            // Assert
            Assert.IsTrue(result.IsSuccessful);

            var usernames = (List<string>)result.ReturnedObject;
            foreach (var username in usernames)
            {
                Console.WriteLine(username); // Print out retrieved usernames
            }


        }
    }
}
