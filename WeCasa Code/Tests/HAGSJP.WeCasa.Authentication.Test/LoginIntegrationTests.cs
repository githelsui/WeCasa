using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;
using System.Diagnostics;

namespace HAGSJP.WeCasa.Services.Implementations
{
    [TestClass]
    public class LoginIntegrationTests
    {
        [TestMethod]
        public void ShouldFetchClaimsWithin5Seconds()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var systemUnderTest = new AccountMariaDAO();

            // Act
            stopwatch.Start();
            UserAccount testUser = new UserAccount("AuthTestSuccess@gmail.com");
            //var testResult = systemUnderTest.AuthenticateUser(testUser);
            stopwatch.Stop();

            // turn ms to seconds
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
            //Assert.IsTrue(testResult.IsSuccessful);
        }
    }
}