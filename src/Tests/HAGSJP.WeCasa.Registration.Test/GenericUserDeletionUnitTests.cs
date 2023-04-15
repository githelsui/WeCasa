using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Managers.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
using System.Net;

namespace HAGSJP.WeCasa.Registration.Test
{
    [TestClass]
    public class GenericUserDeletionUnitTests
    {
        [TestMethod]
        public void ShouldRejectGivenUnauthenticatedSession()
        {
            //Arrange
            List<Claim> initialClaims = new List<Claim>
                {
                    new Claim("Account", "Delete Account"),
                    new Claim("Functionality", "Edit event"),
                    new Claim("Read", "Read files"),
                    new Claim("Write", "Edit note"),
                    new Claim("View", "View section")
                };
            UserAccount ua = new UserAccount("DeleteAcc2@gmail.com", "ACBZ8fMCEK6oBQsM3AT8FdtQWOYRUUmOGEvio5+x98JldraeToc998Yrd551Zg9/ew==");
            UserStatus us = new UserStatus("DeleteAcc2@gmail.com", "ACBZ8fMCEK6oBQsM3AT8FdtQWOYRUUmOGEvio5+x98JldraeToc998Yrd551Zg9/ew==", 2, true, false, false, initialClaims);
            GenericUser gu = new GenericUser();
            Result expected = new Result(false, HttpStatusCode.Unauthorized, "Account Deletion Unsuccessful");

            //Act
            Result actual = gu.DeleteUser(ua, us);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actual.ErrorStatus == expected.ErrorStatus);
            Assert.IsTrue(actual.Message == expected.Message);
        }

        [TestMethod]
        public void ShouldRejectGivenInvalidPermissions()
        {
            //Arrange
            List<Claim> initialClaims = new List<Claim>
                {
                    new Claim("Functionality", "Edit event"),
                    new Claim("Read", "Read files"),
                    new Claim("Write", "Edit note"),
                    new Claim("View", "View section")
                };
            UserAccount ua = new UserAccount("DeleteAcc2@gmail.com", "ACBZ8fMCEK6oBQsM3AT8FdtQWOYRUUmOGEvio5+x98JldraeToc998Yrd551Zg9/ew==");
            UserStatus us = new UserStatus("DeleteAcc2@gmail.com", "ACBZ8fMCEK6oBQsM3AT8FdtQWOYRUUmOGEvio5+x98JldraeToc998Yrd551Zg9/ew==", 2, true, true, false, initialClaims);
            GenericUser gu = new GenericUser();
            Result expected = new Result(false, HttpStatusCode.Unauthorized, "Account Deletion Unsuccessful");

            //Act
            Result actual = gu.DeleteUser(ua, us);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actual.ErrorStatus == expected.ErrorStatus);
            Assert.IsTrue(actual.Message == expected.Message);
        }
    }
}