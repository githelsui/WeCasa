using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Services.Implementations
{
    [TestClass]
    public class AuthenticationUnitTests
    {
        [TestMethod]
        public void ShouldCreateInstanceWithParameterCtor()
        {
            var expected = typeof(Authentication);

            var actual = new Authentication();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }

        [TestMethod]
        public void ShouldCreateInstanceWithParameterCtor2()
        {
            var expected = typeof(Authentication);

            var actual = new Authentication(new AccountMariaDAO());

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }

        [TestMethod]
        public void ShouldVerifyEncryptedPasswordsAuthorized()
        {
            //Arrange
            AuthResult expectedResult = new AuthResult();
            var testHash = new HashSaltSecurity();
            var systemUnderTest = new AccountMariaDAO();
            var hashedPassword = testHash.GetHashSaltCredentials("P@ssw0rd!", "testsalt");
            expectedResult.ExistingAcc = true;
            expectedResult.IsAuth = true;
            expectedResult.IsEnabled = true;
            expectedResult.ReturnedObject = hashedPassword;
            expectedResult.Salt = "testsalt";
            expectedResult.IsSuccessful = false;
            expectedResult.Message = "Account is already authenticated.";
            expectedResult.HasValidCredentials = false;
            AuthResult actualResult = new AuthResult();
            UserAccount testUser = new UserAccount("VerifyEncryptedPasswordTestAlreadyAuthorize@gmail.com", "P@ssw0rd!");

            // Act
            if (!systemUnderTest.ValidateUserInfo(testUser).ExistingAcc)
            {
                systemUnderTest.PersistUser(testUser, hashedPassword, "testsalt");
            }
            systemUnderTest.UpdateUserAuthentication(testUser, true);
            var auth = new Authentication();
            actualResult = auth.VerifyEncryptedPasswords(testUser);

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult == expectedResult);
        }

        [TestMethod]
        public void ShouldVerifyEncryptedPasswordsDisabled()
        {
            //Arrange
            AuthResult expectedResult = new AuthResult();
            var testHash = new HashSaltSecurity();
            var systemUnderTest = new AccountMariaDAO();
            var hashedPassword = testHash.GetHashSaltCredentials("P@ssw0rd!", "testsalt");
            expectedResult.ExistingAcc = true;
            expectedResult.IsAuth = false;
            expectedResult.IsEnabled = false;
            expectedResult.ReturnedObject = hashedPassword;
            expectedResult.Salt = "testsalt";
            expectedResult.IsSuccessful = false;
            expectedResult.Message = "Account Disabled. Perform account recovery or contact the System Administrator.";
            expectedResult.HasValidCredentials = true;
            AuthResult actualResult = new AuthResult();
            UserAccount testUser = new UserAccount("VerifyEncryptedPasswordTestDisabled@gmail.com", "P@ssw0rd!");

            // Act
            if (!systemUnderTest.ValidateUserInfo(testUser).ExistingAcc)
            {
                systemUnderTest.PersistUser(testUser, hashedPassword, "testsalt");
                systemUnderTest.SetUserAbility(testUser, 0);
            }
            var auth = new Authentication();
            actualResult = auth.VerifyEncryptedPasswords(testUser);

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult == expectedResult);
        }

        [TestMethod]
        public void ShouldVerifyEncryptedPasswordsValidateHash()
        {
            //Arrange
            AuthResult expectedResult = new AuthResult();
            var testHash = new HashSaltSecurity();
            var systemUnderTest = new AccountMariaDAO();
            var hashedPassword = testHash.GetHashSaltCredentials("P@ssw0rd!", "testsalt");
            expectedResult.ExistingAcc = true;
            expectedResult.IsAuth = false;
            expectedResult.IsEnabled = true;
            expectedResult.ReturnedObject = hashedPassword;
            expectedResult.Salt = "testsalt";
            expectedResult.Message = "Authentication successful.";
            expectedResult.IsSuccessful = true;
            expectedResult.HasValidCredentials = true;
            AuthResult actualResult = new AuthResult();
            UserAccount testUser = new UserAccount("VerifyEncryptedPasswordTestValidateHash1@gmail.com", "P@ssw0rd!");

            // Act
            if (!systemUnderTest.ValidateUserInfo(testUser).ExistingAcc)
            {
                systemUnderTest.PersistUser(testUser, hashedPassword, "testsalt");
            }
            var auth = new Authentication();
            actualResult = auth.VerifyEncryptedPasswords(testUser);

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult == expectedResult);
        }

        [TestMethod]
        public void ShouldVerifyOTPasswordTooSHort()
        {
            //Arrange
            Result expected = new Result();
            expected.IsSuccessful = false;
            expected.Message = "Invalid username or password provided. Retry again or contact system administrator if issue persists.";
            
            // Act
            var actual = new Authentication();
            Result actualResult = actual.VerifyOTPassword("VerifyOTPasswordTooSHort@gmail.com", "aaaa");

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actualResult.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actualResult.Message == expected.Message);
        }

        [TestMethod]
        public void ShouldVerifyOTPasswordInvalidCharacters()
        {
            //Arrange
            Result expected = new Result();
            expected.IsSuccessful = false;
            expected.Message = "Invalid username or password provided. Retry again or contact system administrator if issue persists.";

            // Act
            var actual = new Authentication();
            Result actualResult = actual.VerifyOTPassword("VerifyOTPasswordTooSHort@gmail.com", "aaaasakfsiw!");

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actualResult.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actualResult.Message == expected.Message);
        }

        [TestMethod]
        public void ShouldUpdateUserAuthenticationFailure()
        {
            //Arrange
            Result expected = new Result();
            expected.IsSuccessful = false;
            expected.Message = $"Rows affected were not 1. It was 0";
            UserAccount testUser = new UserAccount("UpdateUserAuthenticationFailure@gmail.com");

            // Act
            var actual = new Authentication();
            Result actualResult = actual.UpdateUserAuthentication(testUser, false);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actualResult.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actualResult.Message == expected.Message);
        }

        [TestMethod]
        public void ShouldValidateGiven1Attempt()
        {
            //Arrange
            var expected = true;
            var systemUnderTest = new AccountMariaDAO();
            UserAccount testUser = new UserAccount("AccountEnabledTestSuccess@gmail.com");
            UserOperation testOperation = new UserOperation(Operations.Login, 0);
            var log = new Log("Testing if account is enabled (success)", LogLevels.Info, "Data Store", "AccountEnabledTestSuccess@gmail.com", testOperation);

            // Act
            if (systemUnderTest.GetUserOperations(testUser, testOperation).Count == 0)
            {
                var testResult = systemUnderTest.LogData(log);
            }
            var actual = new Authentication();
            bool accountEnabled = actual.IsAccountEnabled(testUser);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(accountEnabled == expected); 
        }

        [TestMethod]
        public void ShouldValidateGiven3Attempts()
        {
            //Arrange
            var expected = false;
            var systemUnderTest = new AccountMariaDAO();
            UserAccount testUser1 = new UserAccount("AccountEnabledTestFailure@gmail.com");
            UserOperation testOperation = new UserOperation(Operations.Login, 0);
            var log = new Log("Testing if account is enabled (failure)", LogLevels.Error, "Data Store", "AccountEnabledTestFailure@gmail.com", testOperation);


            // Act
            if (systemUnderTest.GetUserOperations(testUser1, testOperation).Count == 0)
            {
                systemUnderTest.PersistUser(testUser1, "P@ssw0rd!", "testsalt");
                systemUnderTest.LogData(log);
                systemUnderTest.LogData(log);
                systemUnderTest.LogData(log);
            }
            var actual = new Authentication();
            bool accountEnabled = actual.IsAccountEnabled(testUser1);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(accountEnabled == expected); 
        }
    }
}