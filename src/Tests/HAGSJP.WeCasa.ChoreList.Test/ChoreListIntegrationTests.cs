using System;
using System.Collections;
using System.Diagnostics;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Services.Abstractions;
using HAGSJP.WeCasa.Managers.Implementations;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;
using Mysqlx.Crud;
using Microsoft.AspNetCore.SignalR;

namespace HAGSJP.WeCasa.ChoreList.Test
{
    [TestClass]
    public class ChoreListIntegrationTests
    {
        private ChoreManager _choreManager;

        [TestInitialize]
        public void Initialize()
        {
            _choreManager = new ChoreManager();
        }

        [TestMethod]
        public void ShouldCreateNewChoreWithin5Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            Chore testChore = new Chore()
            {
                ChoreId = 100,
                Name = "Test chore",
                Days = new List<String>() { "MON", "TUES" },
                Notes = null,
                GroupId = 1,
                UsernamesAssignedTo = new List<string>() { "new8@gmail.com", "test@gmail.com" },
                Repeats = null
            };

            // Act
            stopwatch.Start();
            var testResult = _choreManager.AddChore(testChore, new UserAccount("githelsuico@gmail.com"));
            stopwatch.Stop();

            // turn ms to seconds
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(testResult.IsSuccessful);
        }

        [TestMethod]
        public void ShouldEditChoreWithin5Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            Chore testChore = new Chore()
            {
                ChoreId = 100,
                Name = "Test chore new name edit",
                Days = new List<String>() { "MON", "TUES" },
                Notes = null,
                GroupId = 1,
                UsernamesAssignedTo = new List<string>() { "new8@gmail.com", "test@gmail.com" },
                Repeats = null
            };

            // Act
            stopwatch.Start();
            var testResult = _choreManager.EditChore(testChore, new UserAccount("githelsuico@gmail.com"));
            stopwatch.Stop();

            // turn ms to seconds
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(testResult.IsSuccessful);
        }

        [TestMethod]
        public void ShouldCompleteChoreWithin5Secs()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            Chore testChore = new Chore();
            testChore.ChoreId = 100;

            // Act
            stopwatch.Start();
            var testResult = _choreManager.CompleteChore(testChore, new UserAccount("githelsuico@gmail.com"));
            stopwatch.Stop();

            // turn ms to seconds
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(testResult.IsSuccessful);
        }

        [TestMethod]
        public void ShouldDeleteChoreWithin5Secs()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            Chore testChore = new Chore();
            testChore.ChoreId = 100;

            // Act
            stopwatch.Start();
            var testResult = _choreManager.DeleteChore(testChore, new UserAccount("githelsuico@gmail.com"));
            stopwatch.Stop();

            // turn ms to seconds
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(testResult.IsSuccessful);
        }

        [TestMethod]
        public void ShouldFetchGroupToDoChoresWithin5Secs()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;

            // Act
            stopwatch.Start();
            var testResult = _choreManager.GetGroupToDoChores(new GroupModel(1));
            stopwatch.Stop();

            // turn ms to seconds
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(testResult.IsSuccessful);
        }


        [TestMethod]
        public void ShouldFetchGroupCompletedChoresWithin5Secs()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;

            // Act
            stopwatch.Start();
            var testResult = _choreManager.GetGroupCompletedChores(new GroupModel(1));
            stopwatch.Stop();

            // turn ms to seconds
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(testResult.IsSuccessful);
        }
    }
}
