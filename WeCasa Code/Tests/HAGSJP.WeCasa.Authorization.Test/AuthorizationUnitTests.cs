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
        public void ShouldAuthorizeActiveUser()
        {
            //Arrange
            var expected = false;

            //Act
            var actual = new AuthorizationService(new AuthorizationDAO());

            //Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void ShouldAuthorizeFunctionality()
        {
        }

        [TestMethod]
        public void ShouldAuthorizeReadClaim()
        {
        }

        [TestMethod]
        public void ShouldAuthorizeWriteClaim()
        {
        }

        [TestMethod]
        public void ShouldAuthorizeViewAccessClaim()
        {
        }

        [TestMethod]
        public void ShouldUnauthorizeInactiveUser()
        {
        }

        [TestMethod]
        public void LogUnauthorizedAccess()
        {
        }

        [TestMethod]
        public void LogUnauthorizedAccessFailure()
        {
        }
    }
}