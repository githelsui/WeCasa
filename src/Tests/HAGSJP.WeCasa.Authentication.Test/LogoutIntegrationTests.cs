using System;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;
using System.Diagnostics;
using HAGSJP.WeCasa.Models.Security;
using Microsoft.AspNetCore.Identity;

namespace HAGSJP.WeCasa.Authentication.Test
{
    [TestClass]
    public class LogoutIntegrationTests
	{
        [TestMethod]
        public void ShouldLogoutWithin5Seconds()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var accountDAO = new AccountMariaDAO();
            var systemUnderTest = new Logout();
            UserAccount testUser = new UserAccount("AuthorizedUser5Sec@gmail.com");
            accountDAO.PersistUser(testUser, "P@ssw0rd!", "testsalt");
            accountDAO.UpdateUserAuthentication(testUser, true);

            // Act
            stopwatch.Start();
            var testResult = systemUnderTest.LogoutUser(testUser);
            stopwatch.Stop();

            // turn ms to seconds
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(testResult.IsSuccessful);
        }

        [TestMethod]
        public void ShouldSuccessfullyUpdateUserAuth()
        {
            // Arrange
            var expectedIsAuth = false;
            var userManager = new UserManager();
            var accountDAO = new AccountMariaDAO();
            var systemUnderTest = new Logout();
            UserAccount testUser = new UserAccount("AuthorizedUserLogoutUpdatedIsAuth@gmail.com");
            accountDAO.PersistUser(testUser, "P@ssw0rd!", "testsalt");
            OTP otp = userManager.GenerateOTPassword(testUser);
            accountDAO.UpdateUserAuthentication(testUser, true);

            // Act
            var logout = systemUnderTest.LogoutUser(testUser);
            var result = accountDAO.PopulateUserStatus(testUser);
            var message = result.Message;
            UserStatus actual = (UserStatus)result.ReturnedObject;
            var actualIsAuth = actual.IsAuth;


            // Assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actualIsAuth);
            Assert.IsTrue(actualIsAuth == expectedIsAuth);
        }

        [TestMethod]
        public void ShouldNotLogoutUnauthorizedUser()
        {
            // Arrange
            Result expected = new Result();
            expected.IsSuccessful = false;
            expected.Message = "Unable to log out of an account that is not authenticated.";
            var accountDAO = new AccountMariaDAO();
            var systemUnderTest = new Logout();
            UserAccount testUser = new UserAccount("UnauthorizedUserLogout@gmail.com");
            accountDAO.PersistUser(testUser, "P@ssw0rd!", "testsalt");
            accountDAO.UpdateUserAuthentication(testUser, false);

            // Act
            var actual = systemUnderTest.LogoutUser(testUser);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actual.Message == expected.Message);
        }

        [TestMethod]
        public void ShouldLogoutAuthorizedUser()
        {
            // Arrange
            Result expected = new Result();
            expected.IsSuccessful = true;
            expected.Message = string.Empty;
            var accountDAO = new AccountMariaDAO();
            var systemUnderTest = new Logout();
            UserAccount testUser = new UserAccount("AuthorizedUserSuccessfulLogout@gmail.com");
            accountDAO.PersistUser(testUser, "P@ssw0rd!", "testsalt");
            accountDAO.UpdateUserAuthentication(testUser, true);

            // Act
            var actual = systemUnderTest.LogoutUser(testUser);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actual.Message == expected.Message);
        } 
    }
}

