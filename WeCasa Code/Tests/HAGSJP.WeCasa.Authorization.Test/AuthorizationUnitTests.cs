using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Authorization.Test
{
    [TestClass]
    public class AuthorizationUnitTests
    {
        [TestMethod]
        public void ShouldCreateInstanceWithParameterCtor()
        {
            var expected = typeof(AuthorizationService);

            var actual = new AuthorizationService(new AuthorizationDAO());

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }

        [TestMethod]
        public void ShouldUnauthorizeInactiveUser()
        {
            //Arrange
            var expected = new ResultObj();
            expected.IsSuccessful = false;
            expected.Message = "Deny access to unauthorized, logged out user.";

            //Act
            AuthorizationService authService = new AuthorizationService(new AuthorizationDAO());
            //TODO: Edit this specific user has is_enabled set to 0 in the database before testing
            UserAccount testUser = new UserAccount("AuthTestInactiveAcc@gmail.com");
            Claim testClaim = new Claim("Functionality", "Edit event");
            var actual = authService.ValidateClaim(testUser, testClaim);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actual.Message == expected.Message);
        }

        [TestMethod]
        public void ShouldAuthorizeFunctionality()
        {
            //Arrange
            var expected = new ResultObj();
            expected.IsSuccessful = true;
            expected.Message = "Authorized access to Functionality Permissions";
            expected.ReturnedObject = true;

            //Act
            AuthorizationService authService = new AuthorizationService(new AuthorizationDAO());
            //TODO: Edit this specific user has is_enabled set to 1 in the database before testing
            UserAccount testUser = new UserAccount("AuthTestSuccess@gmail.com");
            Claim testClaim = new Claim("Functionality", "Edit event");
            var actual = authService.ValidateClaim(testUser, testClaim);
            bool actualObject = (bool)actual.ReturnedObject;
            bool expectedObject = (bool)expected.ReturnedObject;

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actual.Message == expected.Message);
            Assert.IsTrue(actualObject == expectedObject);
        }

        [TestMethod]
        public void ShouldAuthorizeReadClaim()
        {
            //Arrange
            var expected = new ResultObj();
            expected.IsSuccessful = true;
            expected.Message = "Authorized access to Read Permissions";
            expected.ReturnedObject = true;

            //Act
            AuthorizationService authService = new AuthorizationService(new AuthorizationDAO());
            //TODO: Edit this specific user has is_enabled set to 1 in the database before testing
            UserAccount testUser = new UserAccount("AuthTestSuccess@gmail.com");
            Claim testClaim = new Claim("Read", "Read files");
            var actual = authService.ValidateClaim(testUser, testClaim);
            bool actualObject = (bool)actual.ReturnedObject;
            bool expectedObject = (bool)expected.ReturnedObject;

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actual.Message == expected.Message);
            Assert.IsTrue(actualObject == expectedObject);
        }

        [TestMethod]
        public void ShouldAuthorizeWriteClaim()
        {
            //Arrange
            var expected = new ResultObj();
            expected.IsSuccessful = true;
            expected.Message = "Authorized access to Write Permissions";
            expected.ReturnedObject = true;

            //Act
            AuthorizationService authService = new AuthorizationService(new AuthorizationDAO());
            //TODO: Edit this specific user has is_enabled set to 1 in the database before testing
            UserAccount testUser = new UserAccount("AuthTestSuccess@gmail.com");
            Claim testClaim = new Claim("Write", "Edit note");
            var actual = authService.ValidateClaim(testUser, testClaim);
            bool actualObject = (bool)actual.ReturnedObject;
            bool expectedObject = (bool)expected.ReturnedObject;

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actual.Message == expected.Message);
            Assert.IsTrue(actualObject == expectedObject);
        }

        [TestMethod]
        public void ShouldAuthorizeViewAccessClaim()
        {
            //Arrange
            var expected = new ResultObj();
            expected.IsSuccessful = true;
            expected.Message = "Authorized access to View Permissions";
            expected.ReturnedObject = true;

            //Act
            AuthorizationService authService = new AuthorizationService(new AuthorizationDAO());
            //TODO: Edit this specific user has is_enabled set to 1 in the database before testing
            UserAccount testUser = new UserAccount("AuthTestSuccess@gmail.com");
            Claim testClaim = new Claim("View", "View section");
            var actual = authService.ValidateClaim(testUser, testClaim);
            bool actualObject = (bool)actual.ReturnedObject;
            bool expectedObject = (bool)expected.ReturnedObject;

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actual.Message == expected.Message);
            Assert.IsTrue(actualObject == expectedObject);
        }

        [TestMethod]
        // Testing for user who is logged in, but does not have specific claim.
        public void ShouldUnauthorizeInvalidClaim()
        {
            //Arrange
            var expected = new ResultObj();
            expected.IsSuccessful = true;
            expected.ReturnedObject = false;

            //Act
            AuthorizationService authService = new AuthorizationService(new AuthorizationDAO());
            //TODO: Edit this specific user has is_enabled set to 1 in the database before testing
            UserAccount testUser = new UserAccount("UnauthorizedTest@gmail.com");
            Claim testClaim = new Claim("View", "View section");
            var actual = authService.ValidateClaim(testUser, testClaim);
            bool actualObject = (bool)actual.ReturnedObject;
            bool expectedObject = (bool)expected.ReturnedObject;

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actualObject == expectedObject);
        }

        [TestMethod]
        public void LogUnauthorizedAccessFailure()
        {
        }
    }
}