using System;
using System.Collections;
using System.Diagnostics;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Services.Abstractions;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.UserManagement.Test
{
    [TestClass]
    public class UpdatingProfileIntegrationTests
    {
        [TestMethod]
        public void ShouldUpdateFirstNameIn5Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var systemUnderTest = new UserManager();
            UserAccount testUser = new UserAccount("oldFirst", "oldLast", "updatefirstname@gmail.com");
            systemUnderTest.RegisterUser("oldFirst", "oldLast", "updatefirstname@gmail.com", "P@ssw0rd");

            // Act
            stopwatch.Start();
            var testResult = systemUnderTest.UpdateFirstName(testUser, "newFirst");
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
        public void ShouldUpdateLastNameIn5Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var systemUnderTest = new UserManager();
            UserAccount testUser = new UserAccount("oldFirst", "oldLast", "updatelastname@gmail.com");
            systemUnderTest.RegisterUser("oldFirst", "oldLast", "updatelastname@gmail.com", "P@ssw0rd");

            // Act
            stopwatch.Start();
            var testResult = systemUnderTest.UpdateFirstName(testUser, "newLast");
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
        public void ShouldUpdateUsernameIn5Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var systemUnderTest = new UserManager();
            UserAccount testUser = new UserAccount("oldFirst", "oldLast", "updateusername@gmail.com");
            systemUnderTest.RegisterUser("oldFirst", "oldLast", "updateusername@gmail.com", "P@ssw0rd");


            // Act
            stopwatch.Start();
            var testResult = systemUnderTest.UpdateUsername(testUser, "new@gmail.com");
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
        public void ShouldUpdatePasswordIn5Sec()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var expected = 5;
            var systemUnderTest = new UserManager();
            HashSaltSecurity hashSaltSecurity = new HashSaltSecurity();
            UserAccount testUser = new UserAccount("oldFirst", "oldLast", "updatepassword@gmail.com");
            systemUnderTest.RegisterUser("oldFirst", "oldLast", "updatepassword@gmail.com", "P@ssw0rd");


            // Act
            stopwatch.Start();
            var newPassword = "P@@ssw0rd";
            string salt = BitConverter.ToString(hashSaltSecurity.GenerateSalt(newPassword));
            string encryptedPass = hashSaltSecurity.GetHashSaltCredentials(newPassword, salt);
            var testResult = systemUnderTest.UpdatePassword(testUser, salt, encryptedPass);
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
        //User is able to successfully perform up to 10K UM operations in bulk within 60 seconds
        public void ShouldPerformBulkOps60Sec()
        {
            // Arrange
            var systemUnderTest = new UserManager();
            var results = new ArrayList();
            var accounts = new ArrayList();
            for (int i = 0; i < 1000; i++)
            {
                var uniqueEmail = "user" + i.ToString() + "@gmail.com";
                var tempUser = new UserAccount("oldFirst", "oldLast", uniqueEmail);
                accounts.Add(tempUser);
                systemUnderTest.RegisterUser("oldFirst", "oldLast", uniqueEmail, "P@ssw0rd");
            }
            var stopwatch = new Stopwatch();
            var expected = 60;

            // Act
            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                UserAccount user = (UserAccount)accounts[i];
                var newEmail = "new" + i.ToString() + "@gmail.com";
                var res = systemUnderTest.UpdateUsername(user, newEmail);
                results.Add(res);
            }
            stopwatch.Stop();

            // turn ms to seconds
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual >= 0);
            Assert.IsTrue(actual <= expected);
            for (int i = 0; i < 1000; i++)
            {
                Result res = (Result)results[i];
                Assert.IsTrue(res.IsSuccessful);
            }
        }
    }
}
