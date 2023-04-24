using HAGSJP.WeCasa.Models;

namespace HAGSJP.WeCasa.CircularProgressBar.Test
{
    [TestClass]
    public class CircularProgressBarUnitTests
    {
        [TestMethod]
        public void ShouldCreateModelWithParameterCtor()
        {
            var expected = typeof(ProgressReport);
            var actual = new ProgressReport();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }
    }
}