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
        var expected = false;

        //Act
        Log testLog = new Log("AofQks6zVX7vylbYjfJ4Iu9u5Zd1vr014cZrIyRHSdGTzhF9aAbkGDNOpohAA0zqw3XxJqxO0wxSmJ140A3BXtpLoxvnwv2iscx7Yexy6OlKAru1mXo3tDE9OO23aIJ91k9sowYDRf9TDKPugo3qifVzOA63M5TTCGx2e89kfdNIefCRbiLjNWT1iZbh3TZz3vjwSEwP", "Info", "Business", DateTime.Now, "test_user");
        var logResult = await systemUnderTest.LogData(testLog);
        var actual = logResult.IsSuccessful;

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