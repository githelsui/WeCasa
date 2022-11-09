using HAGSJP.WeCasa.ManagerLayer.Implementations;
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
            Assert.IsTrue(expected == tooShort.IsSuccessful);
            Assert.IsTrue(expected == tooLong.IsSuccessful);
        }

        public void ShouldHaveValidPasswordCharacters()
        {
            //Arrange
            var expected = false;

            //Act
            var actual = new User("test");

            //Assert
            Assert.IsNotNull(actual);
            //Assert.IsTrue(actual.GetType() == expected);
        }

        public void ShouldHaveValidEmailLength()
        {
            //Arrange
            var expected = false;
            
            //Act
            var actual = new User("test");

            //Assert (2 options)
            Assert.IsNotNull(actual);
            //Assert.IsTrue(actual.GetType() == expected);
        }

        public void ShouldHaveValidEmail()
        {
            //Arrange
            var expected = false;

            //Act
            var actual = new User("test");

            //Assert (2 options)
            Assert.IsNotNull(actual);
            //Assert.IsTrue(actual.GetType() == expected);
        }
    }
}