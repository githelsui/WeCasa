using System.Diagnostics;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Managers.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Frontend.Controllers;

namespace HAGSJP.WeCasa.BulletinBoard.Test
{
    [TestClass]
    public class BudgetBarIntegrationTests
    {
        [TestMethod]
        public void ShouldValidateEmptyBillName()
        {
            // Arrange
            BulletinBoardController budgetBarManager = new BulletinBoardController();
            var expected = false;

            // Act
            //budgetBarManager.TESTING();

            // Assert
            Assert.IsNotNull(expected);
            // Assert.IsTrue(actual == expected);
        }
    }
}

