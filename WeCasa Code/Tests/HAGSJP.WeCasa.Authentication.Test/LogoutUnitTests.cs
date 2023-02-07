using System;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Authentication.Test
{
    [TestClass]
    public class LogoutUnitTests
	{
        [TestMethod]
        public void ShouldNotLogoutUnauthorizedUserInitialData()
        {
            //Arrange
            Result expected = new Result();
            expected.IsSuccessful = false;
            expected.Message = "Unable to log out of an account that is not authenticated.";
            var systemUnderTest = new Logout();
            UserAccount testUser = new UserAccount("InitialDataUnauthLogout@gmail.com", "P@ssw0rd!");

            // Act
            var actual = systemUnderTest.LogoutUser(testUser);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actual.Message == expected.Message);
        }

        [TestMethod]
        public void ShouldLogoutAuthorizedUserInitialData()
        {
            //Arrange
            Result expected = new Result();
            expected.IsSuccessful = true;
            expected.Message = string.Empty;
            var systemUnderTest = new Logout();
            UserAccount testUser = new UserAccount("InitialDataAuthLogout@gmail.com", "P@ssw0rd!");

            // Act
            var actual = systemUnderTest.LogoutUser(testUser);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actual.Message == expected.Message);
        }

        [TestMethod]
        public void ShouldRedirectToHomePage()
        {
            
        }

        [TestMethod]
        public void ShouldDisplayConfirmationMessage()
        {
            
        }
    }
}

