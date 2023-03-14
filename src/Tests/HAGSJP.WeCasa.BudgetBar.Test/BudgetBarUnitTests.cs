using HAGSJP.WeCasa.Frontend.Controllers;
namespace HAGSJP.WeCasa.BudgetBar.Test;

[TestClass]
public class UnitTest1
{
    [TestMethod]
        public void ShouldCreateInstanceWithParameterCtor()
        {
            var expected = typeof(BudgetBarController);

            var actual = new BudgetBarController();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }
}
