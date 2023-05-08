
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
using HAGSJP.WeCasa.Managers;
using HAGSJP.WeCasa.Managers.Implementations;
using MySqlX.XDevAPI.Common;

namespace HAGSJP.WeCasa.Reminders.Test
{
    [TestClass]
    public class RemindersIntergrationTest
    {
        private NotificationService? _notificationService;
        private GroupManager? _groupManager;
        private UserManager? _userManager;
        private RemindersDAO? remindersDAO;
        private GroupModel? _testGroup;
        private UserAccount? _testUser1;
        private UserAccount? _testUser2;
        private UserAccount? _testUser3;
        private Chore? _testChore;
        private ChoreManager? _choreManager;
        private Event? _testEvent;
        private CalendarManager? _eventManager;
        private Note? _testNote;
        private BulletinBoardManager? _bulletinManager;
        private Bill? _testbill;
        private BudgetBarManager? _budgetBarManager;



        [TestInitialize]
        public void Initialize()
        {
            _budgetBarManager = new BudgetBarManager();
            _bulletinManager = new BulletinBoardManager();
            _choreManager = new ChoreManager();
            _groupManager = new GroupManager();
            _eventManager = new CalendarManager();
            _userManager = new UserManager();

            var logger = new Logger(new AccountMariaDAO());
            _notificationService = new NotificationService();

            remindersDAO = new RemindersDAO();

            //Adding test items

            _testGroup = new GroupModel()
            {
                GroupName = "Test Group",
                Owner = "wecasacorp@gmail.com",
                Features = new List<string> { "all" },
                Icon = "#668D6A",
                GroupId = 1
            };

            var result = _groupManager.CreateGroup(_testGroup);
            _testGroup.GroupId = result.GroupId;

            _testUser1 = new UserAccount()
            {
                Username = "hvkn19@gmail.com",
                LastName = "name1",
                FirstName = "test1",
                Password = "P@ssWord123"
            };
            _testUser2 = new UserAccount()
            {
                Username = "bly19vy@gmail.com",
                LastName = "name2",
                FirstName = "test2",
                Password = "P@ssWord123"
            };
            _testUser3 = new UserAccount()
            {
                Username = "wecasacorp@gmail.com",
                LastName = "name3",
                FirstName = "test3",
                Password = "P@ssWord123"
            };


            var resultUser1 = _userManager.RegisterUser(_testUser1.FirstName, _testUser1.LastName, _testUser1.Username, _testUser1.Password);
            var resultUser2 = _userManager.RegisterUser(_testUser2.FirstName, _testUser2.LastName, _testUser2.Username, _testUser2.Password);
            var resultUser3 = _userManager.RegisterUser(_testUser3.FirstName, _testUser3.LastName, _testUser3.Username, _testUser3.Password);
            var groupAdd = _groupManager.AddGroupMember(_testGroup, "hvkn19@gmail.com");
            var groupAdd2 = _groupManager.AddGroupMember(_testGroup, "bly19vy@gmail.com");




            _testEvent = new Event()
            {
                EventName = "Test Event",
                Description = "This is a test event",
                GroupId = 1,
                Repeats = default,
                Type = "public",
                Color = "#0256D4",
                CreatedBy = "hvkn19@gmail.com",
                Reminder = "immediately",
                EventDate = DateTime.UtcNow
            };

            _testNote = new Note()
            {
                NoteId = null,
                GroupId = 1,
                Message = "this is a test note",
                DateEntered = DateTime.UtcNow,
                DateModified = null,
                DateDeleted = null,
                LastModifiedUser = "name",
                IsDeleted = null,
                Color = null,
                X = null,
                Y = null,
                PhotoFileName = null

            };



            _testChore = new Chore()
            {
                Name = "Test chore",
                Days = new List<String>() { "MON", "TUES" },
                Notes = "",
                GroupId = 1,
                UsernamesAssignedTo = new List<string>() { "hvkn19@gmail.com", "bly19vy@gmail.com", "wecasacorp@gmail.com" },
                IsCompleted = false,
                Repeats = null
            };

            _testbill = new Bill();
            _testbill.Usernames = new List<string>();
            _testbill.Usernames.Add("hvkn19@gmail.com");
            _testbill.Usernames.Add("bly19vy@gmail.com");
            _testbill.BillName = "Some bill name 1234";
            _testbill.BillDescription = null;
            _testbill.GroupId = 1;
            _testbill.Owner = "wecasacorp@gmail.com";
            _testbill.Amount = 200;
            _testbill.PhotoFileName = "someFile.jpeg";


        }

        [TestMethod]
        public async Task SendGridTest()
        {
            Console.WriteLine("P Sending before email...");
            string from = "wecasacsulb@gmail.com";
            DateTime currentTime = DateTime.Now;
            string to = "hvkn19@Gmail.com";
            string reminderOption = "immediately";
            string eventType = " a test.";
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

        [TestMethod]
        public async Task CalendarReminderTest()
        {
            // Arrange
            var systemUnderTest = new CalendarManager();
            // Act
            var testResult = await systemUnderTest.AddEvent(_testEvent);

            // Assert
            Assert.IsTrue(testResult.IsSuccessful);

        }

        [TestMethod]
        public async Task BulletinBoardTest()
        {
            //Arrange
            var systemUnderTest = new BulletinBoardManager();

            //Act
            var testResult = systemUnderTest.AddNote(_testNote);

            //Assert
            Assert.IsTrue(testResult.IsSuccessful);

        }

        [TestMethod]
        public async Task BillsTest()
        {
            //Arrange
            var systemUnderTest = new BudgetBarManager();

            //Act
            var testResult = _budgetBarManager.InsertBill(_testbill);
            //Assert
            Assert.IsTrue(testResult.IsSuccessful);

        }

        [TestMethod]
        public async Task IncompleteTaskSummaryTest()
        {
            //Arrange
            var systemUnderTest = new ChoreManager();
            var choreTest = _choreManager.AddChore(_testChore, _testUser1);



            //Act
            var testResult = await systemUnderTest.GetGroupIncompleteChores(_testGroup);
            //Assert
            Assert.IsTrue(testResult.IsSuccessful);
        }

        /*[TestCleanup]
        public void Cleanup()
        {
            // Removing group and group files from MariaDB/S3
            _groupManager.DeleteGroup(_testGroup);
            //Remove user and user files from Maria DB/S3
            _userManager.DeleteUser(_testUser);
           // _budgetBarManager.DeleteBill(_testbill);
           // _choreManager.DeleteChore(_testChore);
           // _bulletinManager.DeleteNote(_testNote);
            //_eventManager. how do you delete an event???

        } */

    }
}






