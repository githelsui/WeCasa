using System.Diagnostics;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Managers.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Frontend.Controllers;

namespace HAGSJP.WeCasa.BudgetBar.Test
{
    [TestClass]
    public class BudgetBarIntegrationTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            BudgetBarController systemUnderTest = new BudgetBarController();
            GroupModel testGroup = new GroupModel();
            GroupMariaDAO groupDao = new GroupMariaDAO();
            AccountMariaDAO mariaDAO = new AccountMariaDAO();
            BudgetBarDAO bbDao = new BudgetBarDAO();

            // add group
            testGroup.GroupName = "Test Name";
            testGroup.Owner = "budgetBartest1@gmail.com";
            testGroup.Features = new List<string> {"Budget Bar","Bulletin Board","Calendar","Chore List", "Grocery List", "Circular Progress Bar" };
            testGroup.Icon = "#668D6A";
            
            groupDao.CreateGroup(testGroup);
            var groupId = bbDao.GetGroupId("budgetBartest1@gmail.com");
            testGroup.GroupId = groupId;

            // add users
            for (int i = 1; i <= 5; i++)
            {
                string email = $"budgetBarTest{i}@gmail.com";
                UserAccount ua = new UserAccount(email, "ACBZ8fMCEK6oBQsM3AT8FdtQWOYRUUmOGEvio5+x98JldraeToc998Yrd551Zg9/ew==");
                Result persistResult = mariaDAO.PersistUser(ua, "ACBZ8fMCEK6oBQsM3AT8FdtQWOYRUUmOGEvio5+x98JldraeToc998Yrd551Zg9/ew==", "51-C3-12-5B-9B-00-11-1F-C4-41-AF-23-AB-D7-CB-33");
                // add members to userGroup
                groupDao.AddGroupMember(testGroup, email);
            }
        }

        [TestMethod]
        public void ShouldGetInitialDataWithin5Sec()
        {
            // Arrange
            BudgetBarDAO bbDao = new BudgetBarDAO();
            BudgetBarManager bbManager = new BudgetBarManager();
            var groupId = bbDao.GetGroupId("budgetBartest1@gmail.com");
            var stopwatch = new Stopwatch();
            var expectedTime = 5;

            // Act
            stopwatch.Start();
            var actual = bbManager.GetInitialBudgetBarVew(groupId);
            stopwatch.Stop();
            var actualTime = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actualTime >= 0);
            Assert.IsTrue(actualTime <= expectedTime);
        }

        [TestMethod]
        public void ShouldAddBillWithin5Sec()
        {
            // Arrange
            BudgetBarDAO bbDao = new BudgetBarDAO();
            Bill bill1 = new Bill(new List<string> { "budgetBartest1@gmail.com" }, "budgetBartest1@gmail.com",  0, DateTime.Now, "AddBillWithin5Sec" , "some-description", 10, true, true , false, default, null);            
            var stopwatch = new Stopwatch();
            var expected = 5;

            // Act
            stopwatch.Start();
            var testResult = bbDao.InsertBill(bill1);
            stopwatch.Stop();

            // turn ms to seconds
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(testResult.IsSuccessful);
        }

        [TestMethod]
        public void ShouldUpdateBillWithin5Sec()
        {
            // Arrange result.ReturnedObject
            BudgetBarManager bbManager = new BudgetBarManager();
            BudgetBarDAO bbDao = new BudgetBarDAO();
            Bill bill1 = new Bill(new List<string> { "budgetBartest1@gmail.com" }, "budgetBartest1@gmail.com",  0, DateTime.Now, "UpdateBillWithin5Sec" , "some description", 10, true, true , false, default, null);            
            var insertedBill = bbDao.InsertBill(bill1);
            Bill bill2 = new Bill(new List<string> { "budgetBartest1@gmail.com" }, "budgetBartest1@gmail.com",  (int)insertedBill.ReturnedObject, DateTime.Now, "UpdateBillWithin5Sec" , "some description", 10, true, true , false, default, null);            
            var stopwatch = new Stopwatch();
            var expected = 5;

            // Act
            stopwatch.Start();
            var testResult = bbManager.UpdateBill(bill2);
            stopwatch.Stop();

            // turn ms to seconds
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
            Assert.IsTrue(testResult.IsSuccessful);
        }

        [TestMethod]
        public void ShouldUpdateBudgetWithin5Sec()
        {
            // Arrange
            BudgetBarDAO bbDao = new BudgetBarDAO();
            BudgetBarManager bbManager = new BudgetBarManager();
            var groupId = bbDao.GetGroupId("budgetBartest1@gmail.com");
            var stopwatch = new Stopwatch();
            var expectedTime = 5;

            // Act
            Random rnd = new Random();
            int randomNumber = rnd.Next(10000000);
            stopwatch.Start();
            var testResult = bbManager.EditBudget(groupId, randomNumber);
            stopwatch.Stop();
            var actualResult = bbManager.GetBudget(groupId);

            // turn ms to seconds
            var actualTime = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(testResult);
            Assert.IsTrue(actualTime <= expectedTime);
            Assert.IsTrue(actualResult == randomNumber);
            Assert.IsTrue(testResult.IsSuccessful);
        }  

        // GET
        [TestMethod]
        public void ShouldDeleteBillWithin5Sec()
        {
            // Arrange
            BudgetBarDAO bbDao = new BudgetBarDAO();
            BudgetBarManager bbManager = new BudgetBarManager();
            Bill bill1 = new Bill(new List<string> { "budgetBartest1@gmail.com" }, "budgetBartest1@gmail.com",  0, DateTime.Now, "DeleteBillWithin5Sec" , "some-description", 10, true, true , false, default, null);            
            var groupId = bbDao.GetGroupId("budgetBartest1@gmail.com");
            var stopwatch = new Stopwatch();
            var expectedTime = 5;

            // Act
            Random rnd = new Random();
            int randomNumber = rnd.Next(10000000);
            stopwatch.Start();
            var testResult = bbManager.EditBudget(groupId, randomNumber);
            stopwatch.Stop();
            var actualResult = bbManager.GetBudget(groupId);

            // turn ms to seconds
            var actualTime = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(testResult);
            Assert.IsTrue(actualTime <= expectedTime);
            Assert.IsTrue(actualResult == randomNumber);
            Assert.IsTrue(testResult.IsSuccessful);
        }   

        [TestMethod]
        public void ShouldRestoreBillWithin5Sec()
        {
            // Arrange
            BudgetBarDAO bbDao = new BudgetBarDAO();
            BudgetBarManager bbManager = new BudgetBarManager();
            Bill bill = new Bill(new List<string> { "budgetBartest1@gmail.com" }, "budgetBartest1@gmail.com",  0, DateTime.Now, "RestoreBillWithin5Sec" , "some-description", 10, true, true , true, DateTime.Now, null);            
            DAOResult result = bbDao.InsertBill(bill);
            var stopwatch = new Stopwatch();
            var expectedTime = 5;

            // Act
            stopwatch.Start();
            var testResult = bbManager.RestoreDeletedBill((int) result.ReturnedObject);
            stopwatch.Stop();

            // turn ms to seconds
            var actualTime = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(testResult);
            Assert.IsTrue(actualTime <= expectedTime);
            Assert.IsTrue(testResult.IsSuccessful);
        } 

        [TestMethod]
        public void ShouldCleanDeletedBillsAfter24Hours()
        {
            // Arrange
            BudgetBarDAO bbDao = new BudgetBarDAO();
            Bill bill = new Bill(new List<string> { "budgetBartest1@gmail.com" }, "budgetBartest1@gmail.com",  0, DateTime.Now.AddDays(-2), "CleanDeletedBillsAfter24Hours" , "some-description", 10, true, true , true, default, null);            
            DAOResult insertBillresult = bbDao.InsertBill(bill);
            bbDao.DeleteBill((int)insertBillresult.ReturnedObject, DateTime.Now.AddDays(-1));
            var groupId = bbDao.GetGroupId("budgetBartest1@gmail.com");

            // Act
            Thread.Sleep(TimeSpan.FromMinutes(1));
            List<Bill> bills = bbDao.GetBills(groupId);
            Boolean actual = true;
            foreach(Bill b in bills) 
            {
                if(b.BillId == (int)insertBillresult.ReturnedObject) 
                {
                    actual = false;
                }   
            }

            // Assert
            Assert.IsTrue(actual);
        }   

        [TestMethod]
        public void ShouldCleanHistory()
        {
            // Arrange
            BudgetBarDAO bbDao = new BudgetBarDAO();
            Bill bill = new Bill(new List<string> { "budgetBartest1@gmail.com" }, "budgetBartest1@gmail.com",  0, DateTime.Now.AddMonths(-1), "CleanHistoryUnPaid" , "some-description", 10, false, false , false, default, null);            
            DAOResult insertBillresult = bbDao.InsertBill(bill);
            var groupId = bbDao.GetGroupId("budgetBartest1@gmail.com");

            // Act
            Thread.Sleep(TimeSpan.FromMinutes(1));
            List<Bill> bills = bbDao.GetBills(groupId);
            Boolean actual = true;
            foreach(Bill b in bills) 
            {
                if(b.BillId == (int)insertBillresult.ReturnedObject) 
                {
                    actual = false;
                }   
            }

            // Assert
            Assert.IsTrue(actual);
        }   
    }
}

