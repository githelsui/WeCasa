using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Managers.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using NuGet.Frameworks;

namespace HAGSJP.WeCasa.Registration.Test
{
    [TestClass]
    public class UserCreationUnitTests
    {
        [TestMethod]
        public void ShouldHaveValidPasswordLength()
        {
            //Arrange
            var expected = false;

            //Act
            var actual = new UserManager();
            var tooShort = actual.ValidatePassword("test");
            var tooLong = actual.ValidatePassword("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(tooShort.IsSuccessful == expected);
            Assert.IsTrue(tooLong.IsSuccessful == expected);
        }

        [TestMethod]
        public void ShouldHaveValidPasswordCharacters()
        {
            //Arrange
            var expected = false;

            //Act
            var actual = new UserManager();
            var invalidPassword = actual.ValidatePassword("${}");

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(invalidPassword.IsSuccessful == expected);
        }

        [TestMethod]
        public void ShouldHaveValidEmailLength()
        {
            //Arrange
            var expected = false;
            
            //Act
            var actual = new UserManager();
            var tooLong = actual.ValidateEmail("37k1e57gurB1JUKlL5UGA8xMBA3jj9E7NADhQbtmvrJjKacN28nf8RoWZa2CwGGvtb4SOhURkcq7gELfKBYhSh1nPbigEHUKBemGyOAorXewRpPfaCw6C7B3QrYX7yB6ElQmrye4OgjnrDsiZbPFUvlaPu66o9jeXxpoIwRfV1awAOKflcJfKE4gFRQjDIylZFvYUkTIWqO8GxtSEz6vq8qOW7VKu4illmJrwfuzBlnC3TVLDUaNYHzHP2XDAj5e@gmail.com");
            var notRealEmail = actual.ValidateEmail("wecasatest");

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(tooLong.IsSuccessful == expected);
            Assert.IsTrue(notRealEmail.IsSuccessful == expected);
        }

        [TestMethod]
        public void ShouldHaveValidEmail()
        {
            //Arrange
            var expected = false;

            //Act
            var actual = new UserManager();
            var invalidEmailString = actual.ValidateEmail("&%$@gmail.com");

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(invalidEmailString.IsSuccessful == expected);
        }

        [TestMethod]
        // User must not have an existing account with same email
        // User is therefore not in an active session
        public void ShouldDenyExistingEmail()
        {
            //Arrange
            var result = new AuthResult();
            result.ExistingAcc = true;

            //Act
            var actual = new UserManager();
            var actualResult = actual.IsUsernameTaken("ExistingEmail@gmail.com");

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actualResult == result.ExistingAcc);
        }

        public void ShouldAssignUniqueSystemWideUsername()
        {
            //Arrange
            var result = new AuthResult();
            result.ExistingAcc = false;

            //Act
            var actual = new UserManager();
            var actualResult = actual.IsUsernameTaken("NewEmail@gmail.com");

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actualResult == result.ExistingAcc);
        }

        [TestMethod]
        public void ShouldPersistNewAccountValidCredentials()
        {
            //Arrange
            var expected = new Result();
            expected.IsSuccessful = true;

            //Act
            var actual = new UserManager();
            var actualResult = actual.RegisterUser("validemail@gmail.com", "P@ssw0rd");

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.IsSuccessful == actualResult.IsSuccessful);
        }

    }
}