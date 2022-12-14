using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Services.Implementations
{
    [TestClass]
    public class LoginUnitTests
    {
        [TestMethod]
        public void ShouldReturnAccountEnabledGivenValidNumberOfAttempts()
        {
            //Arrange
            var expected = true;
            var systemUnderTest = new AccountMariaDAO();
            UserAccount testUser = new UserAccount("AccountEnabledTestSuccess@gmail.com");
            UserOperation testOperation = new UserOperation(Operations.Login, 0);
            var log = new Log("Error during Authentication", LogLevels.Error, "Data Store", "AccountEnabledTestSuccess@gmail.com", testOperation);

            // Act
            if (systemUnderTest.GetUserOperations(testUser, testOperation).Count == 0)
            {
                var testResult = systemUnderTest.LogData(log);
            }
            var actual = new Authentication();
            bool accountEnabled = actual.IsAccountEnabled(testUser);

            //Assert
            Assert.IsNotNull(actual);
            Console.WriteLine(accountEnabled);
            Assert.IsTrue(accountEnabled == expected); 
        }

        [TestMethod]
        public void ShouldReturnAccountNotEnabledGivenInvalidNumberOfAttempts()
        {
            //Arrange
            var expected = false;
            var systemUnderTest = new AccountMariaDAO();
            UserAccount testUser = new UserAccount("AccountEnabledTestFailure@gmail.com");
            UserOperation testOperation = new UserOperation(Operations.Login, 0);
            var log = new Log("Error during Authentication", LogLevels.Error, "Data Store", "AccountEnabledTestFailure@gmail.com", testOperation);


            // Act
            if (systemUnderTest.GetUserOperations(testUser, testOperation).Count == 0)
            {
                systemUnderTest.PersistUser(testUser, "P@ssw0rd!", "testsalt");
                systemUnderTest.LogData(log);
                systemUnderTest.LogData(log);
                systemUnderTest.LogData(log);
            }
            var actual = new Authentication();
            bool accountEnabled = actual.IsAccountEnabled(testUser);
            Console.WriteLine("isEnabled " + accountEnabled);

            //Assert
            Assert.IsNotNull(actual);
            Console.WriteLine(accountEnabled);
            Assert.IsTrue(accountEnabled == expected); 
        }

        [TestMethod]
        public void ShouldValidateEncryptedPasswordInLogin()
        {
            //using HashSaltSecurity service within Authentication service
        }
    }
}