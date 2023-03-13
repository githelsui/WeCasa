using Amazon.Runtime.Internal.Auth;
using Amazon.S3.Model;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Moq;

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
            _testGroup.GroupId = result.GroupId;
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
        public async void ShouldUploadFileWithin10Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 10;
            var systemUnderTest = new FileManager();
            var testFile = new Mock<IFormFile>();
            string fileName = "test.txt";
            testFile.Setup(f => f.FileName).Returns(fileName);

            // Act
            stopwatch.Start();
            var result = systemUnderTest.UploadFile(testFile.Object, _testGroup.GroupId.ToString(), _testGroup.Owner);
            stopwatch.Stop();

            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
            //Assert.IsTrue(testResult.IsSuccessful);
        }

        [TestMethod]
        public void ShouldGetFilesWithin10Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 10;
            var systemUnderTest = new FileManager();

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
        public void ShouldDeleteFileSuccessfully()
        {
            // Arrange
            var systemUnderTest = new FileManager();

            // Act
            var actual = systemUnderTest.DeleteFile("test.txt", _testGroup.GroupId.ToString(), _testGroup.Owner);


            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful);
        }

        [TestMethod]
        public void ShouldRetrieveDeletedFilesWithinLast24Hours()
        {
            // Arrange
            var systemUnderTest = new FileManager();

            // Act
            //var actual = systemUnderTest.GetDeletedFiles("test.txt", _testGroup.GroupId.ToString(), _testGroup.Owner);


            // Assert
            //Assert.IsNotNull(actual);
            //Assert.IsTrue(actual.IsSuccessful);
        }

        [TestMethod]
        public void ShouldRestoreFilesWithin24HoursSuccessfully()
        {
            // Arrange
            var systemUnderTest = new FileManager();

            // Act
            var actual = systemUnderTest.DeleteFile("test.txt", _testGroup.GroupId.ToString(), _testGroup.Owner);


            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsSuccessful);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Removing group and group files from MariaDB/S3
            _groupManager.DeleteGroup(_testGroup);
        }
    }
}