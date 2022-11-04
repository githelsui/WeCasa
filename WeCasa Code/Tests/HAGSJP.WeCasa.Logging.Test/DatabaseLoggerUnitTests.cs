namespace HAGSJP.WeCasa.Logging.Test;

using HAGSJP.WeCasa.Logging.Implementations;

[TestClass]
public class UnitTest1
{

    [TestMethod]
    public void ShouldCreateInstanceWithDefaultCtor()
    {
        //Arrange
        var expected = typeof(Logger);

        //Act
        var actual = new DatabaseLogger();

        //Assert (2 options)
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.GetType() == expected);
    }

    [TestMethod]
    public void ShouldCreateInstanceWithParameterCtor()
    {
        //Arrange
        var expected = typeof(Logger);
        var expectedTableName = "someTable";

        //Act
        var actual = new DatabaseLogger(expectedTableName);

        //Assert (2 options)
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.GetType() == expected);
    }

    // Naming Convention 2
    //public void Constructor_CreateNewInstance_Pass()
    //{
    //}


}