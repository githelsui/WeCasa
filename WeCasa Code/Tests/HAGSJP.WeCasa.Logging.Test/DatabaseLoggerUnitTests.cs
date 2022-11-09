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
    public void ShouldCreateInstanceWithValidLogLevelOnly()
    {
        //Arrange
        var expected = false;

        //Act
        Log testLog = new Log("Testing", "Invalid Log Level", "Business", DateTime.Now, "test_user");
        var logResult = await systemUnderTest.LogData(testLog);
        var actual = logResult.IsSuccessful;

        //Assert (2 options)
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.GetType() == expected);
    }

    [TestMethod]
    public void ShouldCreateInstanceWithValidCategoryOnly()
    {
        //Arrange
        var expected = false;

        //Act
        Log testLog = new Log("Testing", "Info", "Invalid Category", DateTime.Now, "test_user");
        var logResult = await systemUnderTest.LogData(testLog);
        var actual = logResult.IsSuccessful;

        //Assert (2 options)
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.GetType() == expected);
    }
}