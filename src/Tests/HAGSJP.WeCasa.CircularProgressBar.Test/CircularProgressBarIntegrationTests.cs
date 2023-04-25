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
        private UserAccount _userAccount;
        private GroupModel _testGroup;
        private GroupManager _groupManager;
        private AccountMariaDAO _daoAccount;
        private GroupMariaDAO _daoGroup;
        private ChoresDAO _dao;
        private Chore _chore;

        [TestInitialize]
        public void Initialize()
        {
            _userAccount = new UserAccount(
                "Test", 
                "User", 
                "circularprogressintegrationtest@gmail.com", 
                "pw491b"
            );

            _testGroup = new GroupModel()
            {
                GroupName = "Test Group 1",
                Owner = "wecasacorp@gmail.com",
                Features = new List<string> { "Circular Progress Bar" },
                Icon = "#668D6A"
            };
            _groupManager = new GroupManager();
            _daoAccount = new AccountMariaDAO();
            _daoGroup = new GroupMariaDAO();
            _dao = new ChoresDAO();
            _chore = new Chore("Test chore", _testGroup.GroupId);

            var createGroupResult = _daoGroup.CreateGroup(_testGroup);
            var testAccountResult = _daoAccount.PersistUser(_userAccount, _userAccount.Password, "saltsaltsalt");
            _testGroup.GroupId = createGroupResult.GroupId;
            var addUserResult = _daoGroup.AddGroupMember(_testGroup, _userAccount.Username);
        }

        [TestMethod]
        public async Task ShouldUpdateOnNewChore()
        {
            // Arrange
            var systemUnderTest = new ChoreService();
            var expectedComplete = 0;
            var expectedIncomplete = 1;

            // Act
            var choreResult = systemUnderTest.AddChore(_chore);
            var result = new ChoreResult();
            result = await systemUnderTest.GetUserProgress(_userAccount.Username, _testGroup.GroupId);
            var actual = result.ChoreProgress;
            
            // Assert
            Assert.IsTrue(choreResult.IsSuccessful);
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsTrue(expectedComplete == actual.CompletedChores);
            Assert.IsTrue(expectedIncomplete == actual.IncompleteChores);
        }

        [TestMethod]
        public async Task ShouldUpdateOnTaskCompletion()
        {
            // Arrange
            var systemUnderTest = new ChoreService();
            _chore.IsCompleted = true;
            var expectedComplete = 1;
            var expectedIncomplete = 1;

            // Act 
            var choreResult = systemUnderTest.EditChore(_chore);
            var result = await systemUnderTest.GetUserProgress(_userAccount.Username, _testGroup.GroupId);
            var actual = result.ChoreProgress;

            // Assert
            Assert.IsTrue(choreResult.IsSuccessful);
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsTrue(expectedComplete == actual.CompletedChores);
            Assert.IsTrue(expectedIncomplete == actual.IncompleteChores);
        }

        [TestMethod]
        public async Task ShouldGetUserProgressWithin3Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 3;
            var systemUnderTest = new ChoreService();

            // Act
            stopwatch.Start();
            var result = await systemUnderTest.GetUserProgress(_userAccount.Username, _testGroup.GroupId);
            stopwatch.Stop();

            var actual = decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _daoGroup.DeleteGroup(_testGroup);
            _daoAccount.DeleteUser(_userAccount);
        }
    }
}