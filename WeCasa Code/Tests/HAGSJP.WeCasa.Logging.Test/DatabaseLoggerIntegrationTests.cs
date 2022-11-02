using System.Diagnostics;
using HAGSJP.WeCasa.Logging.Implementations;

namespace HAGSJP.WeCasa.Logging.Test
{

    [TestClass]
    public class DatabaseLoggerIntegrationTests
    {

        [TestMethod]
        public void ShouldLogWithin5Seconds()
        {
            //Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var systemUnderTest = new DatabaseLogger();

            //Act
            stopwatch.Start();
            var logResult = systemUnderTest.Log("Testing");
            stopwatch.Stop();

            // turn ms to seconds
            var actual = stopwatch.ElapsedMilliseconds * 60_000;

            //Assert (2 options)
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual > 0);
            Assert.IsTrue(actual <= expected);
            //Assert.IsTrue(logResult);
        }

        //[TestMethod]
        //public void ShouldLogWithin5Seconds2()
        //{
        //    //Arrange
        //    var stopwatch = new Stopwatch();
        //    var expected = 5;
        //    var systemUnderTest = new DatabaseLogger();


        //    //Act
        //    try
        //    {
        //        var logResult = systemUnderTest.Log("Testing");
        //        stopwatch.Stop();
        //    } catch(IOException ioex)
        //    {
        //        actual = true;
        //    }

        //    // turn ms to seconds
        //    var actual = stopwatch.ElapsedMilliseconds * 60_000;

        //    //Assert (2 options)
        //    Assert.IsNotNull(actual);
        //    Assert.IsTrue(actual <= expected);
        //    Assert.IsTrue(logResult);
        //}
    }
}