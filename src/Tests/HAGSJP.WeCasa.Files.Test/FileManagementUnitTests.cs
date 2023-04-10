using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Managers.Implementations;

namespace HAGSJP.WeCasa.Files.Test
{
    [TestClass]
    public class FileManagementUnitTests
    {
        [TestMethod]
        public void ShouldCreateInstanceWithParameterCtor()
        {
            var expected = typeof(FileManager);
            var actual = new FileManager();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }

        [TestMethod]
        public void ShouldCreateS3ModelWithParameterCtor()
        {
            var expected = typeof(S3ObjectModel);
            var actual = new S3ObjectModel();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }
    }
}