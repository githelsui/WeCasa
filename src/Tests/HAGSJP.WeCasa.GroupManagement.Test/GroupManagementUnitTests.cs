using System;
using System.Collections;
using System.Diagnostics;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Services.Abstractions;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Managers.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.UserManagement.Test
{
    [TestClass]
    public class GroupManagementUnitTests
    {
        [TestMethod]
        public void ShouldCreateInstanceWithParameterCtor()
        {
            var expected = typeof(GroupManager);

            var actual = new GroupManager();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }

        [TestMethod]
        public void ShouldCreateGroupModelWithParameterCtor()
        {
            var expected = typeof(GroupModel);

            var actual = new GroupModel();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }
    }
}
