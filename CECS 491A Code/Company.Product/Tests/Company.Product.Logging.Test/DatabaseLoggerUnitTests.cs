namespace Company.Product.Logging.Test;
using Company.Product.Logging.Implementations;

[TestClass]
public class UnitTest1
{

    [TestMethod]
    public void ShouldCreateInstanceWithDefaultCtor()
    {
        //Arrange
        var expected = typeof(DatabaseLogger);

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
        var expected = typeof(DatabaseLogger);
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

