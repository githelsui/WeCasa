namespace HAGSJP.WeCasa.Logging.Test;

using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;

[TestClass]
public class DatabaseLoggerUnitTest
{

    [TestMethod]
    public void ShouldCreateInstanceWithDefaultCtor()
    {
        //Arrange
        var expected = typeof(Logger);

        //Act
        var actual = new MariaDBLoggingDAO();

        //Assert (2 options)
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.GetType() == expected);
    }

    [TestMethod]
    public void ShouldCreateInstanceWithParameterCtor()
    {
        //Arrange
        var expected = typeof(Logger);
        var expectedTableName = "Logs";

        //Act
        var actual = new MariaDBLoggingDAO(expectedTableName);

        //Assert (2 options)
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.GetType() == expected);
    }
}