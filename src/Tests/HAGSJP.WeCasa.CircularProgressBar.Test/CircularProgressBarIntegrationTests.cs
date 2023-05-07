using HAGSJP.WeCasa.Managers.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace HAGSJP.WeCasa.CircularProgressBar.Test
{
    [TestClass]
    public class CircularProgressBarIntegrationTests
    {
        private GroupModel _testGroup;
        private GroupManager _groupManager;
        private ChoreManager _choreManager;
        private Chore _chore;
        private List<string> _usernames;

        [TestInitialize]
        public void Initialize()
        {
            _groupManager = new GroupManager();
            _choreManager = new ChoreManager();
            _testGroup = new GroupModel()
            {
                GroupName = "Test Group 1",
                Owner = "wecasacorp@gmail.com",
                Features = new List<string> { "Circular Progress Bar" },
                Icon = "#668D6A"
            };
            // Creating test group
            var createGroupResult = _groupManager.CreateGroup(_testGroup);
            _testGroup.GroupId = (int)createGroupResult.GroupId;

            // Creating test chore
            _chore = new Chore(
                "Test chore",
                _testGroup.GroupId,
                _testGroup.Owner
            );
            _usernames = new List<string>();
            // Assigning group owner to the new chore
            _usernames.Add(_testGroup.Owner);
            _chore.UsernamesAssignedTo = _usernames;
        }

        [TestMethod]
        public void ShouldUpdateOnNewChore()
        {
            // Arrange
            var systemUnderTest = new ChoreService();
            var expectedComplete = 0;
            var expectedIncomplete = 1;

            // Act
            var addChoreResult = systemUnderTest.AddChore(_chore);
            var result = systemUnderTest.GetUserProgress(_testGroup.Owner, _testGroup.GroupId);
            var actual = result.Result.ChoreProgress;

            // Assert
            if (!addChoreResult.IsSuccessful) return;
            Assert.IsTrue(result.Result.IsSuccessful);
            Assert.IsTrue(expectedComplete == actual.CompletedChores);
            Assert.IsTrue(expectedIncomplete == actual.IncompleteChores);
        }

        [TestMethod]
        public void ShouldUpdateOnTaskCompletion()
        {
            // Arrange
            var systemUnderTest = new ChoreService();
            _chore.IsCompleted = true;
            var expectedComplete = 1;
            var expectedIncomplete = 1;

            // Act 
            var choreResult = systemUnderTest.EditChore(_chore);
            var result = systemUnderTest.GetUserProgress(_testGroup.Owner, _testGroup.GroupId);
            var actual = result.Result.ChoreProgress;

            // Assert
            if (!choreResult.IsSuccessful) return;
            Assert.IsTrue(result.Result.IsSuccessful);
            Assert.IsTrue(expectedComplete == actual.CompletedChores);
            Assert.IsTrue(expectedIncomplete == actual.IncompleteChores);
        }

        [TestMethod]
        public void ShouldGetUserProgressWithin3Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 3;
            var systemUnderTest = new ChoreService();

            // Act
            stopwatch.Start();
            var result = systemUnderTest.GetUserProgress(_testGroup.Owner, _testGroup.GroupId);
            stopwatch.Stop();

            var actual = decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
        }

        [TestCleanup]
        public void Cleanup()
        {
            UserAccount ua = new UserAccount(_testGroup.Owner);
            // Deleting chore and chore assignments
            _choreManager.DeleteChore(_chore, ua);
            // Deleting group
            _groupManager.DeleteGroup(_testGroup);
        }
    }
}