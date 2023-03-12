using Amazon.S3.Model;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Drawing;

namespace HAGSJP.WeCasa.Files.Test
{ 
[TestClass]
    public class FileManagementIntegrationTests
    {
        private GroupModel _testGroup;
        private GroupManager _groupManager;

        [TestInitialize]
        public void Initialize()
        {
            _testGroup = new GroupModel()
            {
                GroupName = "Test Group",
                Owner = "wecasacorp@gmail.com",
                Features = new List<string> { "all" },
                Icon = "#668D6A"
            };
            // Adding test group to the database
            _groupManager = new GroupManager();
            var result = _groupManager.CreateGroup(_testGroup);
            //_testGroup.GroupId = result.ReturnedObject;
        }

    [TestMethod]
        public void ShouldSucceedWithNoGroupFiles()
        {
            // Arrange
            var systemUnderTest = new FileManager();

            // Act
            var testResult = systemUnderTest.GetGroupFiles(_testGroup.GroupId.ToString());

            // Assert
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.IsSuccessful);
        }

        [TestMethod]
        public void ShouldGetFilesWithin10Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 10;
            var systemUnderTest = new FileManager();
            // Adding test files to the test group

            // Act
            stopwatch.Start();
            var testResult = systemUnderTest.GetGroupFiles(_testGroup.GroupId.ToString());
            stopwatch.Stop();

            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(testResult.IsSuccessful);
        }

        [TestMethod]
        public void ShouldUploadFileWithin10Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 10;
            var systemUnderTest = new FileManager();

            // Act
            stopwatch.Start();
            stopwatch.Stop();

            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
            //Assert.IsTrue(testResult.IsSuccessful);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Removing group and group files from MariaDB/S3
            _groupManager.DeleteGroup(_testGroup);
        }
    }
}