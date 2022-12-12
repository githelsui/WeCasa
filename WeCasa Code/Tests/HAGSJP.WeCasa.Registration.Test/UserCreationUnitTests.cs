using HAGSJP.WeCasa.Services.Implementations;
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
            Assert.IsTrue(tooLong == expected);
            Assert.IsTrue(notRealEmail == expected);
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
            Assert.IsTrue(invalidEmailString == expected);
        }
    }
}