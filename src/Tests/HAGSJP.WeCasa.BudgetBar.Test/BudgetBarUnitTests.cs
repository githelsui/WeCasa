using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;

namespace HAGSJP.WeCasa.BudgetBar.Test;

[TestClass]
public class BudgetBarUnitTests
{
    [TestMethod]
    public void ShouldCreateInstanceWithParameterCtor()
    {
        var expected = typeof(BudgetBarManager);

        var actual = new BudgetBarManager();

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.GetType() == expected);
    }

    [TestMethod]
    public void ShouldValidateEmptyBillName()
    {
        // Arrange
        Bill bill = new Bill();
        bill.Usernames = new List<string>() {"joy@gmail.com", "wayne@gmail.com"};
        bill.BillName = "";
        bill.BillDescription = "some description";
        bill.Amount = 200;
        bill.PhotoFileName = "someFile.jpeg";
        BudgetBarManager budgetBarManager = new BudgetBarManager();
        var expected = false;

        // Act
        var actual = budgetBarManager.ValidateBill(bill);

        // Assert
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual == expected);
    }

    [TestMethod]
    public void ShouldValidateBillNameInvalidCharacters()
    {
        // Arrange
        Bill bill = new Bill();
        bill.Usernames = new List<string>() {"joy@gmail.com", "wayne@gmail.com"};
        bill.BillName = "!@#$%";
        bill.BillDescription = "some description";
        bill.Amount = 200;
        bill.PhotoFileName = "someFile.jpeg";
        BudgetBarManager budgetBarManager = new BudgetBarManager();
        var expected = false;

        // Act
        var actual = budgetBarManager.ValidateBill(bill);
        Console.Write(actual);

        // Assert
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual == expected);
    }

    [TestMethod]
    public void ShouldValidateValidBillName()
    {
        // Arrange
        Bill bill = new Bill();
        bill.Usernames = new List<string>() {"joy@gmail.com", "wayne@gmail.com"};
        bill.BillName = "Some bill name 1234";
        bill.BillDescription = "some description";
        bill.GroupId = 123456;
        bill.Owner = "joy@gmail.com";
        bill.Amount = 200;
        bill.PhotoFileName = "someFile.jpeg";
        BudgetBarManager budgetBarManager = new BudgetBarManager();
        var expected = true;

        // Act
        var actual = budgetBarManager.ValidateBill(bill);
         Console.WriteLine("dex"+  bill.BillDescription );
         Console.WriteLine("actual"+  actual );

        // Assert
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual == expected);
    }

        [TestMethod]
    public void ShouldValidateBillNameTooLong()
    {
        // Arrange
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string name = new string(Enumerable.Repeat(chars, 61)
        .Select(s => s[random.Next(s.Length)]).ToArray());
        Bill bill = new Bill();
        bill.Usernames = new List<string>() {"joy@gmail.com", "wayne@gmail.com"};
        bill.BillName = name;
        bill.BillDescription = "some description";
        bill.Amount = 200;
        bill.PhotoFileName = "someFile.jpeg";
        BudgetBarManager budgetBarManager = new BudgetBarManager();
        var expected = false;

        // Act
        var actual = budgetBarManager.ValidateBill(bill);

        // Assert
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual == expected);
    }

    [TestMethod]
    public void ShouldValidateBillDescriptionTooLong()
    {
        // Arrange
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string description = new string(Enumerable.Repeat(chars, 2001)
        .Select(s => s[random.Next(s.Length)]).ToArray());
        Bill bill = new Bill();
        bill.Usernames = new List<string>() {"joy@gmail.com", "wayne@gmail.com"};
        bill.BillName = "Some bill name";
        bill.BillDescription = description;
        bill.Amount = 200;
        bill.PhotoFileName = "someFile.jpeg";
        BudgetBarManager budgetBarManager = new BudgetBarManager();
        var expected = false;

        // Act
        var actual = budgetBarManager.ValidateBill(bill);

        // Assert
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual == expected);
    }

    [TestMethod]
    public void ShouldValidateBillDescriptionInvalidCharacters()
    {
        // Arrange
        Bill bill = new Bill();
        bill.Usernames = new List<string>() {"joy@gmail.com", "wayne@gmail.com"};
        bill.BillName = "Some bill name";
        bill.BillDescription = "some description !@#$%";
        bill.Amount = 200;
        bill.PhotoFileName = "someFile.jpeg";
        BudgetBarManager budgetBarManager = new BudgetBarManager();
        var expected = false;

        // Act
        var actual = budgetBarManager.ValidateBill(bill);

        // Assert
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual == expected);
    }

    [TestMethod]
    public void ShouldValidateValidBillDescription()
    {
        // Arrange
        Bill bill = new Bill();
        bill.Usernames = new List<string>() {"joy@gmail.com", "wayne@gmail.com"};
        bill.BillName = "Some bill name";
        bill.BillDescription = "some description 1234";
        bill.Amount = 200;
        bill.PhotoFileName = "someFile.jpeg";
        BudgetBarManager budgetBarManager = new BudgetBarManager();
        var expected = true;

        // Act
        var actual = budgetBarManager.ValidateBill(bill);

        // Assert
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual == expected);
    }

    [TestMethod]
    public void ShouldValidateInvalidBillAmount()
    {
       // Arrange
        Bill bill = new Bill();
        bill.Usernames = new List<string>() {"joy@gmail.com", "wayne@gmail.com"};
        bill.BillName = "Some bill name";
        bill.BillDescription = "some description 1234";
        bill.Amount = 100.1934M;
        bill.PhotoFileName = "someFile.cs";
        BudgetBarManager budgetBarManager = new BudgetBarManager();
        var expected = false;

        // Act
        var actual = budgetBarManager.ValidateBill(bill);

        // Assert
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual == expected);
    }

    [TestMethod]
    public void ShouldValidateValidBillAmount()
    {
       // Arrange
        Bill bill = new Bill();
        bill.Usernames = new List<string>() {"joy@gmail.com", "wayne@gmail.com"};
        bill.BillName = "Some bill name";
        bill.BillDescription = "some description 1234";
        bill.Amount = 20.00M;
        Console.Write(bill.Amount);
        bill.PhotoFileName = "";
        BudgetBarManager budgetBarManager = new BudgetBarManager();
        var expected = true;

        // Act
        var actual = budgetBarManager.ValidateBill(bill);

        // Assert
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual == expected);
    }

    [TestMethod]
    public void ShouldValidateInvalidBillFileExtention()
    {
       // Arrange
        Bill bill = new Bill();
        bill.Usernames = new List<string>() {"joy@gmail.com", "wayne@gmail.com"};
        bill.BillName = "Some bill name";
        bill.BillDescription = "some description 1234";
        bill.Amount = 200;
        bill.PhotoFileName = "someFile.cs";
        BudgetBarManager budgetBarManager = new BudgetBarManager();
        var expected = false;

        // Act
        var actual = budgetBarManager.ValidateBill(bill);

        // Assert
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual == expected);
    }

    [TestMethod]
    public void ShouldValidateValidBillFileExtention()
    {
       // Arrange
        Bill bill = new Bill();
        bill.Usernames = new List<string>() {"joy@gmail.com", "wayne@gmail.com"};
        bill.BillName = "Some bill name";
        bill.BillDescription = "some description 1234";
        bill.Amount = 200;
        bill.PhotoFileName = "someFile.jpeg";
        BudgetBarManager budgetBarManager = new BudgetBarManager();
        var expected = true;

        // Act
        var actual = budgetBarManager.ValidateBill(bill);

        // Assert
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual == expected);
    }

    [TestMethod]
    public void ShouldValidateInvalidId()
    {
       // Arrange
        BudgetBarManager budgetBarManager = new BudgetBarManager();
        var expected = false;

        // Act
        var actual = budgetBarManager.ValidateId(-1);

        // Assert
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual == expected);
    }

    [TestMethod]
    public void ShouldValidateValidId()
    {
       // Arrange
        BudgetBarManager budgetBarManager = new BudgetBarManager();
        var expected = true;

        // Act
        var actual = budgetBarManager.ValidateId(12345);

        // Assert
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual == expected);
    }
}
