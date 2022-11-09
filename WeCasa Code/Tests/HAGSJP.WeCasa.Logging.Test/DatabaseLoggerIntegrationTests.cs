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
        public async void ShouldLogWithin5Seconds()
        {
            //Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var systemUnderTest = new MariaDbDAO();

            //Act
            stopwatch.Start();
            var logResult = await systemUnderTest.LogData("Testing", "Info", "Business", DateTime.Now, 0);
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