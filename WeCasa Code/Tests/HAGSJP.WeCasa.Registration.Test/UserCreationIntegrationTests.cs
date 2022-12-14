using System.Diagnostics;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Registration.Test
{
    [TestClass]
    public class UserCreationIntegrationTests
    {
        [TestMethod]
        public void ShouldCreateWithin5Seconds()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var systemUnderTest = new AccountMariaDAO();

            // Act
            stopwatch.Start();
            UserAccount testUser = new UserAccount("5secondintegration@gmail.com");
            var testResult = systemUnderTest.PersistUser(testUser, "P@ssw0rd", "testsalt");
            stopwatch.Stop();

            // turn ms to seconds
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds,60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual > 0);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(testResult.IsSuccessful);
        }

        [TestMethod] //Tests that no error log has been made for user creation that takes less than 5 seconds
        public void ShoudLogOnlySuccessRegistrationTakesMoreThan5Seconds()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expectedTime = 5;
            var message = "Account created successfully, but took longer than 5 seconds";
            var expected = false;
            var dao = new AccountMariaDAO();
            var um = new UserManager(dao);

            // Act
            stopwatch.Start();
            var testResult = um.RegisterUser("MoreThan5Seconds@gmail.com", "P@ssw0rd");
            stopwatch.Stop();
            var testAcount = new UserAccount("MoreThan5Seconds@gmail.com");
            List<Log> logs = dao.GetLogData(testAcount, Operations.Registration);

            // search for existing message in logs. actual = false if there are no logs with the specific message
            var actual = false;
            foreach(Log log in logs)
            {
                if (log.Message.Equals(message))
                {
                    actual = true;
                }
            }

            var time = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(time);
            Assert.IsNotNull(time > 0);
            Assert.IsNotNull(time <= expectedTime);
            Assert.IsNotNull(logs);
            Assert.IsTrue(expected == actual); // No error logs were made because user creation took less than 5 seconds
        }

    }
}