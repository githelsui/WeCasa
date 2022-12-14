using System.Diagnostics;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Authorization.Test
{
    [TestClass]
    public class AuthorizationIntegrationTests
    {
        [TestMethod]
        public void ShouldFetchClaimsWithin5Seconds()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var systemUnderTest = new AuthorizationDAO();

            // Act
            stopwatch.Start();
            UserAccount testUser = new UserAccount("AuthTestSuccess@gmail.com");
            var testResult = systemUnderTest.GetClaims(testUser);
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
        public void ShouldFetchRolesWithin5Seconds()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var systemUnderTest = new AuthorizationDAO();

            // Act
            stopwatch.Start();
            UserAccount testUser = new UserAccount("AuthTestSuccess@gmail.com");
            var testResult = systemUnderTest.GetRole(testUser);
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
        public void ShouldValidateClaimWithin5Secs()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var systemUnderTest = new AuthorizationService(new AuthorizationDAO());

            // Act
            stopwatch.Start();
            UserAccount testUser = new UserAccount("AuthTestSuccess@gmail.com");
            Claim testClaim = new Claim("View", "View section");
            var testResult = systemUnderTest.ValidateClaim(testUser, testClaim);
            stopwatch.Stop();

            // turn ms to seconds
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual > 0);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(testResult.IsSuccessful);
        }
    }
}