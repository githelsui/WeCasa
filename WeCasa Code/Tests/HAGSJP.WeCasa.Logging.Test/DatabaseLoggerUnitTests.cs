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
        var actual = new MariaDbDAO();

        //Assert (2 options)
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.GetType() == expected);
    }

    [TestMethod]
    public void ShouldCreateInstanceWithValidCharacters()
    {
        //Arrange
        var expected = typeof(Logger);

        //Act
        var actual = new MariaDbDAO();

        //Assert (2 options)
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.GetType() == expected);
    }

    [TestMethod]
    public void ShouldCreateInstanceWithValidLength()
    {
        //Arrange
        var expected = typeof(Logger);

        //Act
        var actual = new MariaDbDAO();

        //Assert (2 options)
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.GetType() == expected);
    }

    [TestMethod]
    public void ShouldCreateInstanceWithValidLogLevels()
    {
        //Arrange
        var expected = typeof(Logger);

        //Act
        var actual = new MariaDbDAO();

        //Assert (2 options)
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.GetType() == expected);
    }

    [TestMethod]
    public void ShouldCreateInstanceWithValidCategories()
    {
        //Arrange
        var expected = typeof(Logger);

        //Act
        var actual = new MariaDbDAO();

        //Assert (2 options)
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.GetType() == expected);
    }
}