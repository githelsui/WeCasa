using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Services.Implementations
{
    [TestClass]
    public class LoginUnitTests
    {
        [TestMethod]
        public void ShouldValidateEncryptedPasswordInLogin()
        {
            //Arrange
            var expected = true;
            var systemUnderTest = new AccountMariaDAO();
            
            UserAccount testUser = new UserAccount("EncryptedPasswordTest@gmail.com", "P@ssw0rd!");
            // Password Encryption
            HashSaltSecurity hashService = new HashSaltSecurity();
            string salt = BitConverter.ToString(hashService.GenerateSalt(testUser.Password));
            string encryptedPass = hashService.GetHashSaltCredentials(testUser.Password, salt);
            var testResult = systemUnderTest.PersistUser(testUser, encryptedPass, salt);


            // Act
            Authentication auth = new Authentication();
            var actual = auth.VerifyEncryptedPasswords(testUser);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected);
        }
        [TestMethod]
        public void ShouldRejectInvalidEmail()
        {
            //Arrange
            var expected = false;
            var systemUnderTest = new AccountMariaDAO();
            UserManager userManager = new UserManager();
            Authentication auth = new Authentication();
            UserAccount testUser = new UserAccount("EncryptedPasswordTest", "P@ssw0rd!");

            // Act
            Login login = new Login();
            var actual = login.LoginUser(testUser, auth, userManager);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected);
        }

        [TestMethod]
        public void ShouldRejectPassword()
        {
            // TODO
        }

        [TestMethod]
        public void ShouldRejectInvalidOTP()
        {
            // TODO
        }

        [TestMethod]
        public void ShouldRejectExpiredOTP()
        {
            // TODO
        }

        [TestMethod]
        public void ShouldUpdateAuthenticationInDB()
        {
            // TODO
        }

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
    }
}