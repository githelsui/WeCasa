using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;
using System.Diagnostics;

namespace HAGSJP.WeCasa.Services.Implementations
{
    [TestClass]
    public class LoginIntegrationTests
    {
        [TestMethod]
        public void ShouldDisplaySuccessMessageSuccessfulLogin()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expectedTime = 5;
            var expectedSuccess = true;
            var message = "Successfully logged user in.";
            UserManager userManager = new UserManager();
            var systemUnderTest = new Authentication();

            // Act
            stopwatch.Start();
            UserAccount testUser = new UserAccount("AuthTestSuccess@gmail.com");
            OTP testOTP = userManager.GenerateOTPassword(testUser);
            var actual = systemUnderTest.AuthenticateUser(testUser, testOTP, testOTP);
            stopwatch.Stop();

            // turn ms to seconds
            var time = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(time >= 0);
            Assert.IsTrue(time <= expectedTime);
            Assert.IsTrue(actual.Message.Equals(message)); //Display message is correct
            Assert.IsTrue(actual.IsSuccessful == expectedSuccess);
        }
    }
}