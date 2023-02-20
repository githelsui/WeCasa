using System;
using System.Collections;
using System.Diagnostics;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Services.Abstractions;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.UserManagement.Test
{
    [TestClass]
    public class GroupManagementIntegrationTests
    {
        [TestMethod]
        public void ShouldCreateNewGroupWithin5Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var userManager = new UserManager();
            var systemUnderTest = new GroupManager();
            var testGroup = new GroupModel();
            testGroup.GroupName = "Test Name";
            testGroup.Owner = "createnewgroup3@gmail.com";
            testGroup.Features = new List<string> { "all" };
            testGroup.Icon = "#668D6A";
            userManager.RegisterUser("first", "last", "createnewgroup3@gmail.com", "P@ssw0rd");


            // Act
            stopwatch.Start();
            var testResult = systemUnderTest.CreateGroup(testGroup);
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
        public void ShouldDeleteGroupWithin5Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var userManager = new UserManager();
            var systemUnderTest = new GroupManager();
            var testGroup = new GroupModel();
            var groupOwner = new UserAccount("createnewgroup4@gmail.com");
            testGroup.GroupName = "New Group";
            testGroup.Owner = "createnewgroup3@gmail.com";
            testGroup.Features = new List<string> { "all" };
            testGroup.Icon = "#668D6A";
            testGroup.GroupId = 80;
            userManager.RegisterUser("first", "last", "createnewgroup4@gmail.com", "P@ssw0rd");
            systemUnderTest.CreateGroup(testGroup);

            // Act
            stopwatch.Start();
            var testResult = systemUnderTest.DeleteGroup(groupOwner, testGroup);
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
        public void ShouldAddNewValidGroupMemberWithin5Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var userManager = new UserManager();
            var systemUnderTest = new GroupManager();
            var testGroup = new GroupModel();
            testGroup.GroupName = "Test Name";
            testGroup.Owner = "GroupOwner@gmail.com";
            testGroup.Features = new List<string> { "all" };
            testGroup.Icon = "#668D6A";
            userManager.RegisterUser("first", "last", "UserToBeAdded3@gmail.com", "P@ssw0rd");

            // Act
            stopwatch.Start();
            var testResult = systemUnderTest.AddGroupMember(testGroup, "UserToBeAdded3@gmail.com");
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
        public void ShouldRemoveExistingGroupMemberWithin5Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var userManager = new UserManager();
            var systemUnderTest = new GroupManager();
            var testGroup = new GroupModel();
            testGroup.GroupName = "Test Name";
            testGroup.Owner = "GroupOwner@gmail.com";
            testGroup.Features = new List<string> { "all" };
            testGroup.Icon = "#668D6A";
            userManager.RegisterUser("first", "last", "UserToBeRemoved4@gmail.com", "P@ssw0rd");
            systemUnderTest.AddGroupMember(testGroup, "UserToBeRemoved4@gmail.com");


            // Act
            stopwatch.Start();
            var testResult = systemUnderTest.RemoveGroupMember(testGroup, "UserToBeRemoved4@gmail.com");
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
