using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;

namespace HAGSJP.WeCasa.Services.Implementations
{
    [TestClass]
    public class LoginUnitTests
    {
        [TestMethod]
        public void ShouldReturnTrueGivenValidNumberOfAttempts()
        {
           //Arrange
            var expected = true;
            List<DateTime> failedAttemptTimes = new List<DateTime>();
            failedAttemptTimes.Add(DateTime.Now.AddHours(-23));
            failedAttemptTimes.Add(DateTime.Now.AddHours(-1));

            //Act
            var actual = new Authentication();
            /*bool accountEnabled = actual.IsAccountEnabled(failedAttemptTimes);

            //Assert
            Assert.IsNotNull(actual);
            Console.WriteLine(accountEnabled);
            Assert.IsTrue(accountEnabled == expected); */
        }

        [TestMethod]
        public void ShouldLogInvalidAttemps()
        {
            //Arrange
            var expected = true;
            List<DateTime> failedAttemptTimes = new List<DateTime>();
            failedAttemptTimes.Add(DateTime.Now.AddHours(-23));
            failedAttemptTimes.Add(DateTime.Now.AddHours(-1));

            //Act
            var actual = new Authentication();
            /*bool accountEnabled = actual.IsAccountEnabled(failedAttemptTimes);

            //Assert
            Assert.IsNotNull(actual);
            Console.WriteLine(accountEnabled);
            Assert.IsTrue(accountEnabled == expected); */
        }

        [TestMethod]
        public void ShouldReturnFalseGivenInvalidNumberOfAttempts()
        {
            //Arrange
            var expected = false;
            List<DateTime> failedAttemptTimes = new List<DateTime>();
            failedAttemptTimes.Add(DateTime.Now.AddHours(-23));
            failedAttemptTimes.Add(DateTime.Now.AddHours(-1));
            failedAttemptTimes.Add(DateTime.Now.AddHours(-.5));

            //Act
            var actual = new Authentication();
            UserAccount testUser = new UserAccount("AuthTestInactiveAcc@gmail.com");
            bool accountEnabled = actual.IsAccountEnabled(testUser);

            //Assert
            Assert.IsNotNull(actual);
            Console.WriteLine(accountEnabled);
            Assert.IsTrue(accountEnabled == expected);
        }
    
    }
}