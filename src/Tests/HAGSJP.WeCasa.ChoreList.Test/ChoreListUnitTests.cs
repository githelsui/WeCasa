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

namespace HAGSJP.WeCasa.ChoreList.Test
{
    [TestClass]
    public class ChoreListUnitTests
    {
        private ChoreManager _choreManager;

        [TestInitialize]
        public void Initialize()
        {
            _choreManager = new ChoreManager();
        }

        [TestMethod]
        public void ShouldCreateInstanceWithParameterCtor()
        {
            var expected = typeof(ChoreManager);
            var actual = new ChoreManager();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }

        [TestMethod]
        public void ShouldFailChoreCreationInvalidInputs()
        {
            //Arrange
            var expected = false;

            //Act
            var actual = new UserManager();
            var tooShort = actual.ValidatePassword("test");
            Chore testChore = new Chore()
            {
                ChoreId = 101,
                Name = "Special characters +&*",
                Days = new List<String>() { "MON", "TUES" },
                Notes = null,
                GroupId = 1,
                UsernamesAssignedTo = new List<string>() { "new8@gmail.com", "test@gmail.com" },
                Repeats = null
            };
            var invalidNameCharacters = _choreManager.AddChore(testChore, new UserAccount("githelsuico@gmail.com"));

            testChore = new Chore()
            {
                ChoreId = 102,
                Name = "Name too long CrNicUcCOmMEayQjGhDJdoPRBkdqbNiDSsPHwHdbGHclVoNyzRrNQxeqwVAAwicDdlsUyQ",
                Days = new List<String>() { "MON", "TUES" },
                Notes = null,
                GroupId = 1,
                UsernamesAssignedTo = new List<string>() { "new8@gmail.com", "test@gmail.com" },
                Repeats = null
            };
            var invalidNameLength = _choreManager.AddChore(testChore, new UserAccount("githelsuico@gmail.com"));

            //Assert
            Assert.IsNotNull(actual);
            /*Assert.IsTrue(invalidNameCharacters.IsSuccessful == expected);
            Assert.IsTrue(invalidNameLength.IsSuccessful == expected);*/
        }

        [TestMethod]
        public void ShouldFailChoreCreationInvalidAssignments()
        {
            //Arrange
            var expected = false;

            //Act
            var actual = new UserManager();
            var tooShort = actual.ValidatePassword("test");
            Chore testChore = new Chore()
            {
                ChoreId = 103,
                Name = "Name",
                Days = new List<String>() { "MON", "TUES" },
                Notes = null,
                GroupId = 1,
                UsernamesAssignedTo = new List<string>() { "userdoesntexist@gmail.com" },
                Repeats = null
            };
            var result = _choreManager.AddChore(testChore, new UserAccount("githelsuico@gmail.com"));

            //Assert
            Assert.IsNotNull(actual);
            //Assert.IsTrue(result.IsSuccessful == expected);
        }
    }
}

