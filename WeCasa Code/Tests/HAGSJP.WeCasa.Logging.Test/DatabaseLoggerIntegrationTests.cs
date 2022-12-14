using System.Diagnostics;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Logging.Test
{

    [TestClass]
    public class DatabaseLoggerIntegrationTest
    {

        [TestMethod]
        public async Task ShouldLogWithin5Seconds()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var testMariaDao = new AccountMariaDAO();

            // Act
            stopwatch.Start();
            Logger systemUnderTest = new Logger(testMariaDao);
            var logResult = await systemUnderTest.Log("Testing", LogLevels.Info, "Business", "test_user");
            stopwatch.Stop();

            // turn ms to seconds
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual > 0);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(logResult.IsSuccessful);
        }
    }
}