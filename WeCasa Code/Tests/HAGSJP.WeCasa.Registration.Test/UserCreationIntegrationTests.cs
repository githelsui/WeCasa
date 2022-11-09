using System.Diagnostics;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Registration.Test
{
    [TestClass]
    public class UserCreationIntegrationTests
    {
        [TestMethod]
        public void ShouldCreateWithin5Seconds()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var systemUnderTest = new MariaDbDAO();

            // Act
            stopwatch.Start();
            User testUser = new User("5secondintegration@gmail.com");
            var testResult = systemUnderTest.AddUser(testUser, "P@ssw0rd");
            stopwatch.Stop();

            // turn ms to seconds
            var actual = stopwatch.ElapsedMilliseconds * 60_000;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual > 0);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(testResult.IsSuccessful);
        }
    }
}