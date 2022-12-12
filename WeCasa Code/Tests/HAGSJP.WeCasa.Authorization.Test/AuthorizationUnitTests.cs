using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
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
            expected.IsSuccessful = true;
            expected.ReturnedObject = false;
            expected.Message = "Successfully denied access to unauthorized, logged out user.";

            //Act
            AuthorizationService authService = new AuthorizationService(new AuthorizationDAO());
            UserAccount testUser = new UserAccount("AuthTestInactiveAcc@gmail.com"); //User has is_enabled = 0
            var actual = authService.ValidateActiveUser(testUser);
            bool actualBool = (bool)actual.ReturnedObject;
            bool expectedBool = (bool)expected.ReturnedObject;

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actualBool == expectedBool);
            Assert.IsTrue(actual.Message == expected.Message);
        }

        [TestMethod]
        public void ShouldUpdateClaims()
        {
            //Arrange
            var expected = new ResultObj();
            expected.IsSuccessful = true;
            expected.Message = "Successfully updated claims for user.";

            //Act
            AuthorizationService authService = new AuthorizationService(new AuthorizationDAO());
            UserAccount testUser = new UserAccount("AuthTestSuccess@gmail.com"); //User has is_enabled = 1      
            // Initial claims when new user is first registered
            List<Claim> newClaims = new List<Claim>
                {
                    new Claim("Functionality", "Edit event"),
                    new Claim("Read", "Read files"),
                    new Claim("Write", "Edit note"),
                    new Claim("View", "View section")
                };
            var actual = authService.AddClaims(testUser, newClaims);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actual.Message == expected.Message);
        }

        [TestMethod]
        public void ShouldAuthorizeFunctionalityClaim()
        {
            //Arrange
            var expected = new ResultObj();
            expected.IsSuccessful = true;
            expected.Message = "Authorized access to Functionality Permissions.";
            expected.ReturnedObject = true;

            //Act
            AuthorizationService authService = new AuthorizationService(new AuthorizationDAO());
            UserAccount testUser = new UserAccount("AuthTestSuccess@gmail.com"); //User has is_enabled = 1
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
            expected.Message = "Authorized access to Read Permissions.";
            expected.ReturnedObject = true;

            //Act
            AuthorizationService authService = new AuthorizationService(new AuthorizationDAO());
            UserAccount testUser = new UserAccount("AuthTestSuccess@gmail.com"); //User has is_enabled = 1
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
            expected.Message = "Authorized access to Write Permissions.";
            expected.ReturnedObject = true;

            //Act
            AuthorizationService authService = new AuthorizationService(new AuthorizationDAO());
            UserAccount testUser = new UserAccount("AuthTestSuccess@gmail.com"); //User has is_enabled = 1
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
            expected.Message = "Authorized access to View Permissions.";
            expected.ReturnedObject = true;

            //Act
            AuthorizationService authService = new AuthorizationService(new AuthorizationDAO());
            UserAccount testUser = new UserAccount("AuthTestSuccess@gmail.com"); //User has is_enabled = 1
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
        // Testing for user who is logged in, but does not have target claim.
        public void ShouldUnauthorizeInvalidClaim()
        {
            //Arrange
            var expected = new ResultObj();
            expected.IsSuccessful = true;
            expected.ReturnedObject = false;

            //Act
            AuthorizationService authService = new AuthorizationService(new AuthorizationDAO());
            UserAccount testUser = new UserAccount("UnauthorizedTest@gmail.com"); //User has is_enabled = 1
            Claim testClaim = new Claim("View", "View section");
            var actual = authService.ValidateClaim(testUser, testClaim);
            bool actualObject = (bool)actual.ReturnedObject;
            bool expectedObject = (bool)expected.ReturnedObject;

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful == expected.IsSuccessful);
            Assert.IsTrue(actualObject == expectedObject);
        }
    }
}