using System.Diagnostics;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Logging.Test
{

    [TestClass]
    public class DatabaseLoggerIntegrationTest
    {

        [TestMethod]
        public void ShouldLogWithin5Seconds()
        {
            //Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var systemUnderTest = new MariaDBLoggingDAO("test");

            //Act
            stopwatch.Start();
            var logResult = systemUnderTest.LogData("Testing");
            stopwatch.Stop();

            // turn ms to seconds
            var actual = stopwatch.ElapsedMilliseconds * 60_000;

            //Assert (2 options)
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual > 0);
            Assert.IsTrue(actual <= expected);
            //Assert.IsTrue(logResult);
        }
    }
}