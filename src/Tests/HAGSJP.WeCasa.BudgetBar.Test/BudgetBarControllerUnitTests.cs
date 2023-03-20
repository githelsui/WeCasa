using HAGSJP.WeCasa.Frontend.Controllers;
using HAGSJP.WeCasa.Services.Implementations;

namespace HAGSJP.WeCasa.BudgetBar.Test;

[TestClass]
public class BudgetBarControllerUnitTests
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
