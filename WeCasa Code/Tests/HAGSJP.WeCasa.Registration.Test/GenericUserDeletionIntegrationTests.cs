using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.sqlDataAccess;
using System.Net;

namespace HAGSJP.WeCasa.Registration.Test
{
    [TestClass]
    public class GenericUserDeletionIntegrationTests
    {
        [TestMethod]
        public void ShouldSuccessfullyDeleteUserFromDatabase()
        {
            // Arrange
            UserAccount ua = new UserAccount("DeleteAcc2@gmail.com", "ACBZ8fMCEK6oBQsM3AT8FdtQWOYRUUmOGEvio5+x98JldraeToc998Yrd551Zg9/ew==");
            AccountMariaDAO mariaDAO = new AccountMariaDAO();
            List<Claim> initialClaims = new List<Claim>
                {
                    new Claim("Account", "Delete Account"),
                    new Claim("Functionality", "Edit event"),
                    new Claim("Read", "Read files"),
                    new Claim("Write", "Edit note"),
                    new Claim("View", "View section")
                };
            UserStatus us = new UserStatus("DeleteAcc2@gmail.com", "ACBZ8fMCEK6oBQsM3AT8FdtQWOYRUUmOGEvio5+x98JldraeToc998Yrd551Zg9/ew==", 2, true, true, false, "Ws.eWwX5R8",  DateTime.UtcNow, initialClaims);
            GenericUser gu = new GenericUser();
            Result expected = new Result(true, HttpStatusCode.OK, "Account Deletion Successful");

            // Act
            Result persistResult = mariaDAO.PersistUser(ua, "ACBZ8fMCEK6oBQsM3AT8FdtQWOYRUUmOGEvio5+x98JldraeToc998Yrd551Zg9/ew==", "51-C3-12-5B-9B-00-11-1F-C4-41-AF-23-AB-D7-CB-33");
            Result actual = gu.DeleteUser(ua, us);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.IsSuccessful == actual.IsSuccessful);
            Assert.IsTrue(expected.ErrorStatus == actual.ErrorStatus);
            Assert.IsTrue(expected.Message == actual.Message);
        }
    }
}